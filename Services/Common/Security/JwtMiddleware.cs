namespace WebApi.Services.Common.Security;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context,TokenService tokenService)
    {
        string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        (int? UserId, int? StoreId) jwtToken = tokenService.ValidateJwtToken(token);
        if (jwtToken is { UserId: not null, StoreId: not null })
        {
            context.Items["User"] = jwtToken.UserId;
            context.Items["Store"] = jwtToken.StoreId;
        }
        
        await _next(context);
    }
}