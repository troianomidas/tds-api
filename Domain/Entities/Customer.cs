using WebApi.Domain.Common;
using WebApi.Domain.Messages;

namespace WebApi.Domain.Entities;

public class Customer : BaseStoreEntity
{
    public Customer()
    {
        
    }

    public Customer(int storeId, string? name, string? phone)
    {
        if (storeId < 1)
            throw new InvalidOperationException(InputMsg.Required, new Exception("storeId"));
        
        if (string.IsNullOrEmpty(name))
            throw new InvalidOperationException(InputMsg.Required, new Exception("name"));
        
        if (name.Length is < 8 or > 40)
            throw new InvalidOperationException(InputMsg.LengthMin8Max40, new Exception("name"));
        
        if (string.IsNullOrEmpty(phone))
            throw new InvalidOperationException(InputMsg.Required, new Exception("phone"));
        
        if (phone.Length is < 10 or > 11)
            throw new InvalidOperationException(InputMsg.LengthMin10Max11, new Exception("phone"));
        
        StoreId = storeId;
        Name = name.ToUpper();
        Phone = phone;
    }
    
    public string? ExternalId { get; set; }
    public string? Name { get; set; }
    public string Phone { get; set; }
    public string? Email { get; set; }
    public string? Document { get; set; }
}