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

public class GetAnnouncementById : IRequest<Announcement?>
{
    public int StoreId { get; set; }
    public int AnnouncementId { get; set; }
}

public class GetAnnouncementByIdHandler : IRequestHandler<GetAnnouncementById, Announcement?>
{
    private readonly AppDbContext _context;

    public GetAnnouncementByIdHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Announcement?> Handle(GetAnnouncementById request,
        CancellationToken cancellationToken)
    {
        if (request.StoreId < 1)
            throw new BadRequestException("StoreId is required.");

        return await _context.Announcements.Where(x => x.StoreId == request.StoreId && x.Id == request.AnnouncementId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}