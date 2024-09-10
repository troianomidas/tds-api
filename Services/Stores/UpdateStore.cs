using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using WebApi.Services.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Domain.Constants;
using WebApi.Domain.Entities;
using WebApi.Domain.Exceptions;
using WebApi.Integrations.Queues;

namespace WebApi.Services.Stores;

public record UpdateStoreRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Phone { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? Category { get; set; }
    public StoreAddress? Address { get; set; }
    public User? User { get; set; }
}

public class UpdateStoreRequestHandler : IRequestHandler<UpdateStoreRequest, bool>
{
    private readonly AppDbContext _context;
    private readonly IQueue _queue;

    public UpdateStoreRequestHandler(AppDbContext context, IQueue queue)
    {
        _context = context;
        _queue = queue;
    }

    public async Task<bool> Handle(UpdateStoreRequest request, CancellationToken cancellationToken)
    {
        Store? storeDb = await _context.Stores.Where(x => x.Id == request.StoreId)
            .Include(x => x.Address)
            .Include(x => x.User)
            .FirstOrDefaultAsync(cancellationToken);

        if (storeDb?.User == null)
            throw new InvalidOperationException("User/Store not found");

        var store = new Store(storeDb.UserId, request.Name, request.Phone, request.Category)
        {
            Description = request.Description,
            LogoUrl = request.LogoUrl,
            BannerUrl = request.BannerUrl,
        };

        store.GenerateHostname();
        
        var user = new User(request.User?.Name, request.User?.Document, request.User?.Email);

        storeDb.User.Name = user.Name;
        storeDb.User.Document = user.Document;
        storeDb.User.Email = user.Email;

        var address = new StoreAddress(request.Address?.Zipcode, request.Address?.Line1, request.Address?.Number,
            request.Address?.Neighborhood, request.Address?.CityState)
        {
            Line2 = request.Address?.Line2
        };

        storeDb.Name = store.Name;
        storeDb.Description = store.Description;
        storeDb.LogoUrl = store.LogoUrl;
        storeDb.BannerUrl = store.BannerUrl;
        storeDb.Phone = store.Phone;
        storeDb.Category = store.Category;
        storeDb.Hostname = store.Hostname;

        if (storeDb.Address == null)
            storeDb.Address = address;
        else
        {
            storeDb.Address.Zipcode = address.Zipcode;
            storeDb.Address.Line1 = address.Line1;
            storeDb.Address.Line2 = address.Line2;
            storeDb.Address.Number = address.Number;
            storeDb.Address.Neighborhood = address.Neighborhood;
            storeDb.Address.CityState = address.CityState;
        }
        
        //onboarding
        var createBilling = false;
        if (storeDb.Status == StoreStatusConst.Pending)
        {
            storeDb.Status = store.Status;
            createBilling = true;
        }

        bool isSaved = await _context.SaveChangesAsync(cancellationToken) > 0;

        if (isSaved && createBilling)
            await _queue.SendMessageAsync(QueueConst.GenerateSubscriptionBillingQueue,
                JsonConvert.SerializeObject(new
                {
                    StoreId = storeDb.Id,
                    user.Name,
                    user.Document,
                    storeDb.User.WhatsappNumber,
                    user.Email,
                    Amount = 4,
                    DueDate = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd")
                }));

        return isSaved;
    }
}