using WebApi.Domain.Entities.Collaborators;
using WebApi.Persistence;
using MediatR;

namespace WebApi.Services.Collaborators;

public class CreateCollaboratorRequest : IRequest<bool>
{
    public int StoreId { get; set; }
    public string? Name { get; set; }
    public string? GroupName { get; set; }
    public string? Document { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
}

public class CreateCollaboratorRequestHandler : IRequestHandler<CreateCollaboratorRequest, bool>
{
    private readonly AppDbContext _context;

    public CreateCollaboratorRequestHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateCollaboratorRequest request, CancellationToken cancellationToken)
    {
        var collaborator = new Collaborator(request.StoreId, request.Name, request.Phone, request.GroupName)
        {
            Email = request.Email,
            Document = request.Document,
            Description = request.Description
        };

        _context.Collaborators.Add(collaborator);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}