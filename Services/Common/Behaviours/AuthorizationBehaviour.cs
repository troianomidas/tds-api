using System.Reflection;
using MediatR;

namespace WebApi.Services.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly int? _storeId;

    public AuthorizationBehaviour(
        IHttpContextAccessor  httpContext)
    {
        if (httpContext.HttpContext == null)
            return;
        
        _storeId = (int)(httpContext.HttpContext.Items["Store"] ?? 0);
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        IList<PropertyInfo> props = new List<PropertyInfo>(request.GetType().GetProperties());
        
        if(_storeId > 0)
            props.FirstOrDefault(x => x.Name == "StoreId")
                ?.SetValue(request, _storeId);
        
        return await next();
    }
}