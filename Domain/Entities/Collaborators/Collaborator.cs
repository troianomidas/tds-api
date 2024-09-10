using WebApi.Domain.Common;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities.Collaborators;

public class Collaborator : BaseStoreEntity
{
    public Collaborator()
    {
        
    }

    public Collaborator(int storeId, string? name, string? phone, string? groupName)
    {
        if(storeId < 1)
            throw new InvalidOperationException("Informe o 'Codigo da loja' do colaborador");
        
        if (string.IsNullOrEmpty(name))
            throw new InvalidOperationException("Informe o 'Nome completo' do colaborador");
        
        if(name.Split(" ").Length < 1)
            throw new InvalidOperationException("Informe o 'Nome completo' do colaborador");
        
        if(string.IsNullOrEmpty(phone))
            throw new InvalidOperationException("Informe o 'Contato' do colaborador");
        
        if(phone.Length < 13)
            throw new InvalidOperationException("Informe o 'Contato' do colaborador");
        
        if(string.IsNullOrEmpty(groupName))
            throw new InvalidOperationException("Informe o 'Grupo' do colaborador");

        StoreId = storeId;
        Name = name;
        Phone = phone;
        GroupName = groupName;
        CreatedAt = DateTimeUtils.Now();
        Status = 1;
    }
    
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? GroupName { get; set; }
    public string? Document { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public DateTime? UpdatedAt { get; set; }
}