using AutoMapper;
using WebApi.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services.Financials;

public class GetFinancialsRequest : IRequest<DashboardFinancialModel>
{
    public int StoreId { get; set; }
    public DateTime DateFilterDate { get; set; }
}

public class GetFinancialsRequestHandler : IRequestHandler<GetFinancialsRequest, DashboardFinancialModel>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetFinancialsRequestHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DashboardFinancialModel> Handle(GetFinancialsRequest request, CancellationToken cancellationToken)
    {
        var startMonthDate = new DateTime(request.DateFilterDate.Year, request.DateFilterDate.Month, 1);
        var endMonthDate = startMonthDate.AddMonths(1).AddDays(-1);
        
        var dashboardModelResponse = new DashboardFinancialModel();
        
        var orderList = await _context.Orders.Where(x => x.StoreId == request.StoreId && x.CreatedAt >= startMonthDate && x.CreatedAt <= endMonthDate && x.Status == 5)
            .Include(x=>x.PaymentMethod).OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        if (orderList.Count <= 0)
            return null!;
        
        dashboardModelResponse.TotalOrders = orderList.Count;
        dashboardModelResponse.GrossValue = Math.Round(orderList.Sum(x=> x.TotalValue), 2);
        // dashboardModelResponse.StoreValue = Math.Round(orderList.Where(x=> x.PaymentMethod!.IsOnline == false).Sum(x => x.TotalValue), 2);
        // dashboardModelResponse.OnlineValue = Math.Round(orderList.Where(x=> x.PaymentMethod!.IsOnline).Sum(x => x.TotalValue), 2);
        // dashboardModelResponse.NetValue = Math.Round(dashboardModelResponse.GrossValue - dashboardModelResponse.OnlineValue * (decimal)0.07);
        
        foreach (var orderPerDay in orderList.GroupBy(x=>x.CreatedAt.Date))
        {
            dashboardModelResponse.HistoryProfitsPerDate.Add(Math.Round(orderPerDay.Sum(x=>x.TotalValue)));
            dashboardModelResponse.HistoryDates.Add(orderPerDay.Key.ToShortDateString());
        }

        // var pineChartValuesModel = new List<PineChartValuesModel>
        // {
        //     new() { HistoryGroupDates = "Monday", Color = "#00FF00"},
        //     new() { HistoryGroupDates = "Tuesday", Color = "#FF0000" },
        //     new() { HistoryGroupDates = "Wednesday", Color = "#00b2ff" },
        //     new() { HistoryGroupDates = "Thursday", Color = "#FF00FF" },
        //     new() { HistoryGroupDates = "Friday", Color = "#8A2BE2" },
        //     new() { HistoryGroupDates = "Saturday", Color = "#FFFF00" },
        //     new() { HistoryGroupDates = "Sunday", Color = "#D2691E" },
        // };
        
        // foreach (var pineChartValue in pineChartValuesModel)
        // {
        //     pineChartValue.HistoryProfitBalance = Math.Round(orderList.Where(x=> x.CreatedAt.Date.DayOfWeek.ToString() == pineChartValue.HistoryGroupDates!)
        //         .Sum(x => x.TotalValue / orderList.Count(y => y.CreatedAt.Date.DayOfWeek.ToString() == pineChartValue.HistoryGroupDates!)), 2);
        // }
        
        // dashboardModelResponse.PineChartValueModels.AddRange(pineChartValuesModel);
        
        return dashboardModelResponse;
    }
}

public class DashboardFinancialModel
{
    public int TotalOrders { get; set; }
    public decimal GrossValue { get; set; }
    // public decimal NetValue { get; set; }
    // public decimal OnlineValue { get; set; }
    // public decimal StoreValue { get; set; }
    public List<decimal> HistoryProfitsPerDate { get; set; } = new();
    public List<string> HistoryDates { get; set; } = new();
    
    // public List<PineChartValuesModel> PineChartValueModels { get; set; } = new();
}

// public class PineChartValuesModel
// {
//     public decimal HistoryProfitBalance { get; set; }
//     public string? HistoryGroupDates { get; set; }
//     public string? Color { get; set; }
// }