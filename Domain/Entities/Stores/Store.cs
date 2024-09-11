using WebApi.Domain.Common;
using WebApi.Domain.Constants;
using WebApi.Domain.Entities.Subscriptions;
using WebApi.Domain.Messages;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities.Stores;

public class Store : BaseEntity
{
    public Store()
    {
        
    }

    public Store(string? name, string? phone, string? ownerName, string? ownerDocument, int? referralCodeId, decimal planAmount)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidOperationException("Preencha o nome da loja.");

        if (name.Length is < 4 or > 45)
            throw new InvalidOperationException("O nome da loja deve ter entre 4 e 45 caracteres.");

        if (string.IsNullOrEmpty(phone))
            throw new InvalidOperationException("Número de telefone inválido.");

        if (phone.Trim().Length < 14 || phone.Trim().Length > 20)
            throw new InvalidOperationException("Número de telefone inválido.");
        
        if (string.IsNullOrEmpty(ownerName))
            throw new InvalidOperationException("Preencha o nome completo.");

        if (ownerName.Split(" ").Length < 2)
            throw new InvalidOperationException("Preencha o nome completo.");

        if (ownerName.Length > 65)
            throw new InvalidOperationException("O nome completo deve ter no máximo 65 caracteres.");

        if (string.IsNullOrEmpty(ownerDocument))
            throw new InvalidOperationException("Preencha CPF do responsável legal.");

        if (!CpfCnpj.IsCpfValid(ownerDocument))
            throw new InvalidOperationException("Informe um CPF válido.");
        
        ExternalId = Guid.NewGuid().ToString();
        Name = name.Trim();
        Phone = phone;
        OwnerName = ownerName.Trim();
        OwnerDocument = ownerDocument;
        Status = StoreStatusConst.Active;
        
        GenerateHostname();
        
        StoreDelivery = new StoreDelivery(Id, false, (decimal)7.00)
        {
            HasDelivery = true,
            HasWithdraw = true,
            DeliveryTimeMin = 45,
            DeliveryTimeMax = 60,
            WithdrawTimeMin = 35,
            WithdrawTimeMax = 50,
        };

        StoreSettings = new StoreSettings
        {
            ExternalId = ExternalId,
            FilterOrderDateType = 1,
            FilterOrderSortAsc = 2,
            FilterOrderSortType = 1
        };

        Subscription = new Subscription(Id, referralCodeId, planAmount);
        
        StorePaymentMethods = new List<StorePaymentMethod>();
        StorePaymentMethods.Add(new StorePaymentMethod(Id, 2));
        StorePaymentMethods.Add(new StorePaymentMethod(Id, 22));
        StorePaymentMethods.Add(new StorePaymentMethod(Id, 32));
        
        OpeningHours = new List<OpeningHour>();

        foreach (DayOfWeek dayOfWeek in new List<DayOfWeek>{
                     DayOfWeek.Monday,
                     DayOfWeek.Tuesday,
                     DayOfWeek.Wednesday,
                     DayOfWeek.Thursday,
                     DayOfWeek.Friday,
                     DayOfWeek.Saturday,
                     DayOfWeek.Sunday,
                 })
        {
            OpeningHours.Add(new OpeningHour(Id, dayOfWeek.ToString(), "08:00", "23:00", 0, 1));
        }

        Announcements = new List<Announcement>();
    }

    public Store(int userId, string? name, string? phone, string? category)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidOperationException("Por favor, preencha o campo 'Nome da loja'.");

        if (name.Length is < 4 or > 65)
            throw new InvalidOperationException("O campo 'Nome da loja' deve ter entre 4 e 65 caracteres.");

        if (string.IsNullOrEmpty(phone))
            throw new InvalidOperationException("Por favor, preencha o campo 'Telefone ou celular'.");

        if (phone.Length is < 14 or > 15)
            throw new InvalidOperationException("O campo 'Telefone ou celular' deve ter entre 14 e 15 caracteres.");
        
        if (string.IsNullOrEmpty(category))
            throw new InvalidOperationException("Por favor, preencha o campo 'Especialidade'.");
       
        UserId = userId;
        Name = name;
        Phone = phone;
        Category = category;
        Status = StoreStatusConst.Active;
        GenerateHostname();
    }

    public int UserId { get; set; }
    public string? Name { get; set; }
    public string? ExternalId { get; set; }
    public string? Description { get; set; }
    public string? Phone { get; set; }
    public string? Hostname { get; set; }
    public string? Category { get; set; }
    public string? LogoUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? OwnerName { get; set; }
    public string? OwnerDocument { get; set; }
    public int Status { get; set; }
    public int ReviewCount { get; set; }
    public double ReviewRate { get; set; }
    public StoreDelivery? StoreDelivery { get; set; }
    public Review? Review { get; set; }
    public StoreAddress? Address { get; set; }
    public StoreSettings? StoreSettings { get; set; }
    public User? User { get; set; }
    public Subscription? Subscription { get; set; }
    public ICollection<StorePaymentMethod>? StorePaymentMethods { get; set; }
    public ICollection<OpeningHour>? OpeningHours { get; set; }
    public ICollection<DeliveryArea>? DeliveryAreas { get; set; }
    public ICollection<ScheduledBreak>? ScheduledBreaks { get; set; }
    public ICollection<Announcement>? Announcements { get; set; }

    public void GenerateHostname()
    {
        if (string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(Hostname))
            return;

        string hostname = Name.Trim().ToLower() + $"{DateTimeUtils.Now().Minute}{DateTimeUtils.Now().Millisecond}";

        foreach (string split in hostname.Split(" "))
            Hostname += split;
    }
}