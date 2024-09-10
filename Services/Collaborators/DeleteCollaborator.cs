using WebApi.Domain.Entities.Collaborators;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Collaborators;

public class DeleteCollaboratorRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public int CollaboratorId { get; set; }
}

public class DeleteCollaboratorRequestHandler : IRequestHandler<DeleteCollaboratorRequest, bool>
{
    private readonly AppDbContext _context;

    public DeleteCollaboratorRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteCollaboratorRequest request, CancellationToken cancellationToken)
    {
        Collaborator? collaborator = await _context.Collaborators.Where(x => x.StoreId == request.StoreId && x.Id == request.CollaboratorId)
            .FirstOrDefaultAsync(cancellationToken);

        if (collaborator == null)
            throw new InvalidOperationException("Colaborador nao encontrado");
        
        collaborator.Status = 9;

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}