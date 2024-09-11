using WebApi.Domain.Entities.Stores;
using WebApi.Persistence;
using WebApi.Services.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Domain.Constants;
using WebApi.Domain.Entities;
using WebApi.Domain.Exceptions;
// using WebApi.Integrations.Queues;

namespace WebApi.Services.Stores;

public record UpdateStoreRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Hostname { get; set; }
    public string? Phone { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? Category { get; set; }
    public string? OwnerName { get; set; }
    public StoreAddress? Address { get; set; }
}

public class UpdateStoreRequestHandler : IRequestHandler<UpdateStoreRequest, bool>
{
    private readonly AppDbContext _context;

    public UpdateStoreRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateStoreRequest request, CancellationToken cancellationToken)
    {
        Store? storeDb = await _context.Stores.Where(x => x.Id == request.StoreId)
            .Include(x => x.Address)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (storeDb == null)
            throw new InvalidOperationException("User/Store not found");
        
        var store = new Store(request.Name, request.Phone, request.OwnerName, storeDb.OwnerDocument, null, 0)
        {
            Description = request.Description?.Trim(),
            LogoUrl = request.LogoUrl,
            BannerUrl = request.BannerUrl,
            Hostname = request.Hostname?.Trim(),
            Category = request.Category
        };
       
        var address = new StoreAddress(request.Address?.Zipcode, request.Address?.State, request.Address?.City,
            request.Address?.Neighborhood, request.Address?.Line1, request.Address?.Number, request.Address?.Line2)
        {
            Line2 = request.Address?.Line2
        };
        
        storeDb.Name = store.Name;
        storeDb.Description = store.Description;
        storeDb.Category = store.Category;
        storeDb.LogoUrl = store.LogoUrl;
        storeDb.BannerUrl = store.BannerUrl;
        storeDb.Phone = store.Phone;
        storeDb.Hostname = store.Hostname;
        storeDb.OwnerName = store.OwnerName;
        
        if (storeDb.Address == null)
            storeDb.Address = address;
        else
        {
            storeDb.Address.Zipcode = address.Zipcode;
            storeDb.Address.State = address.State;
            storeDb.Address.City = address.City;
            storeDb.Address.Neighborhood = address.Neighborhood;
            storeDb.Address.Line1 = address.Line1;
            storeDb.Address.Number = address.Number;
            storeDb.Address.Line2 = address.Line2;
        }
        
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}