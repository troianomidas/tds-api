using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Services.Common.Models;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Services.Common.Security;

public class TokenService
{
    private readonly ILogger<TokenService> _logger;

    public TokenService(ILogger<TokenService> logger) => _logger = logger;

    private const int ExpirationHours = 6;
    private const string Secret = "m%hUFBvdyX7xZsl68PMt*z$";

    public string GenerateJwtToken(int userId, int storeId)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, "Delivery3Api"),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.GetCultureInfo("pt-BR"))),
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.GroupSid, storeId.ToString())
            }),
            Expires = DateTimeUtils.Now().AddHours(ExpirationHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret)), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public (int?, int?) ValidateJwtToken(string? token)
    {
        if (token == null)
            return (null, null);

        var tokenHandler = new JwtSecurityTokenHandler();
        
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return (int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value),
                int.Parse(jwtToken.Claims.First(x => x.Type == "groupsid").Value));
        }
        catch
        {
            return (null, null);
        }
    }
}