using System.Diagnostics;
using Azure;
using Azure.Data.Tables;
using MediatR;

namespace WebApi.Services.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly TableClient _tableClient;
    private readonly IHttpContextAccessor _httpContext;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger, IConfiguration configuration,
        IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
        _tableClient = new TableClient(configuration.GetConnectionString("Storage"), "apilogs");
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var trace = new StackTrace(ex, true);
            var storeId = (int)(_httpContext.HttpContext?.Items["Store"] ?? 0);
            var userId = (int)(_httpContext.HttpContext?.Items["User"] ?? 0);
            _ = await _tableClient.AddEntityAsync(new TablePartition
            {
                RowKey = Guid.NewGuid().ToString(),
                PartitionKey = "errors",
                StoreId = storeId,
                UserId = userId,
                ClassName = trace.GetFrame(0)?.GetFileName(),
                Message = ex.Message,
                MethodName = trace.GetFrame(0)?.GetMethod()?.ReflectedType?.FullName,
                RowNumber = trace.GetFrame(0)?.GetFileLineNumber() ?? 0
            }, cancellationToken);

            throw;
        }
    }
}

public record TablePartition : ITableEntity
{
    public string RowKey { get; set; } = default!;
    public string PartitionKey { get; set; } = default!;
    public ETag ETag { get; set; } = default!;
    public DateTimeOffset? Timestamp { get; set; } = default!;
    public string Message { get; set; } = null!;
    public string? ClassName { get; set; } = null!;
    public int RowNumber { get; set; } = default!;
    public int UserId { get; set; } = default!;
    public int StoreId { get; set; } = default!;
    public string? MethodName { get; set; } = null!;
}