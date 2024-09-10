using WebApi.Domain.Entities.Collaborators;
using WebApi.Persistence;
using WebApi.Services.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Collaborators;

public class EditCollaboratorRequest : IRequest<bool>
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public string? Name { get; set; }
    public string? GroupName { get; set; }
    public string? Document { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
}

public class EditCollaboratorRequestHandler : IRequestHandler<EditCollaboratorRequest, bool>
{
    private readonly AppDbContext _context;

    public EditCollaboratorRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(EditCollaboratorRequest request, CancellationToken cancellationToken)
    {
        Collaborator? collaboratorDb = await _context.Collaborators.Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
        if (collaboratorDb == null)
            throw new InvalidOperationException("Colaborador nao encontrado");
        
        var collaborator = new Collaborator(request.StoreId, request.Name, request.Phone, request.GroupName)
        {
            Email = request.Email,
            Document = request.Document,
            Description = request.Description,
            UpdatedAt = DateTimeUtils.Now()
        };

        collaboratorDb.Name = collaborator.Name;
        collaboratorDb.Phone = collaborator.Phone;
        collaboratorDb.Email = collaborator.Email;
        collaboratorDb.Document = collaborator.Document;
        collaboratorDb.GroupName = collaborator.GroupName;
        collaboratorDb.Description = collaborator.Description;
        collaboratorDb.UpdatedAt = collaborator.UpdatedAt;

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}