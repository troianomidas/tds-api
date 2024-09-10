using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Domain.Entities.Stores;
using WebApi.Domain.Exceptions;

namespace WebApi.Services.OpeningHours;

public record DeleteOpeningHourRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public int OpeningHourId { get; set; }
}

public class DeleteOpeningHourRequestHandler : IRequestHandler<DeleteOpeningHourRequest, bool>
{
    private readonly AppDbContext _context;

    public DeleteOpeningHourRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteOpeningHourRequest request, CancellationToken cancellationToken)
    { 
        await _context.OpeningHours.Where(x => x.Id == request.OpeningHourId && x.StoreId == request.StoreId).ExecuteDeleteAsync(cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}