using WebApi.Domain.Common;
using WebApi.Services.Common.Models;

namespace WebApi.Domain.Entities;

public class OpeningHour : BaseStoreEntity
{
    public OpeningHour()
    {
        
    }

    public OpeningHour(int storeId, string? dayOfWeek, string? beginAt, string? endAt, int scheduleType, int sort)
    {
        if (string.IsNullOrEmpty(dayOfWeek))
            throw new InvalidOperationException("Informe o dia da semana.");
        
        BeginAt = beginAt;
        EndAt = endAt;
        
        if(DateTimeBegin() >= DateTimeEnd())
            throw new InvalidOperationException("O horário inicial deve ser menor que o horário final.");
        
        StoreId = storeId;
        DayOfWeek = dayOfWeek;
       
        ScheduleType = scheduleType;
        Sort = sort;
        CreatedAt = DateTimeUtils.Now();
    }

    public string? DayOfWeek { get; set; }
    public string? BeginAt { get; set; }
    public string? EndAt { get; set; }
    public int ScheduleType { get; set; }
    public int Sort { get; set; }
    
    public DateTime DateTimeBegin()
    {
        return DateTime.Parse($"{DateTimeUtils.Now().ToShortDateString()} {BeginAt}");
    }
    
    public DateTime DateTimeEnd()
    {
        return DateTime.Parse($"{DateTimeUtils.Now().ToShortDateString()} {EndAt}");
    }
}