using AutoMapper;
using AutoMapper.QueryableExtensions;
using WebApi.Domain.Entities;
using WebApi.Domain.Exceptions;
using WebApi.Persistence;
using WebApi.Services.Common.Mappings;
using WebApi.Services.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Services.Common.Interfaces;

namespace WebApi.Services.Announcements;

public class ListAnnouncementsRequest : IRequest<List<Announcement>>
{
    public int StoreId { get; set; }
}

public class ListAnnouncementsHandler : IRequestHandler<ListAnnouncementsRequest, List<Announcement>>
{
    private readonly AppDbContext _context;

    public ListAnnouncementsHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Announcement>> Handle(ListAnnouncementsRequest request,
        CancellationToken cancellationToken)
    {
        if (request.StoreId < 1)
            throw new BadRequestException("StoreId is required.");

        return await _context.Announcements.Where(x => x.StoreId == request.StoreId)
            .ToListAsync(cancellationToken);
    }
}