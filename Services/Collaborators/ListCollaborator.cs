using WebApi.Domain.Entities.Collaborators;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Collaborators;

public class ListCollaboratorRequest : IRequest<List<Collaborator>>
{
    public int StoreId { get; set; }
    public string? GroupName { get; set; }
    public int Status { get; set; }
}

public class ListCollaboratorRequestHandler : IRequestHandler<ListCollaboratorRequest, List<Collaborator>>
{
    private readonly AppDbContext _context;

    public ListCollaboratorRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Collaborator>> Handle(ListCollaboratorRequest request, CancellationToken cancellationToken)
    {
        IQueryable<Collaborator> query = _context.Collaborators.Where(x => x.StoreId == request.StoreId);

        if (!string.IsNullOrEmpty(request.GroupName))
            query = query.Where(x => x.GroupName == request.GroupName);

        if(request.Status > 0)
            query = query.Where(x => x.Status == request.Status);
        
        return await query
            .ToListAsync(cancellationToken);
    }
}