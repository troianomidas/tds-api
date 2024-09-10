using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WebApi.Services.Common.Security;

public struct PasswordHelper
{
    public static string Encode(string password)
    {
        byte[] bytes = "quantso_encrypt"u8.ToArray();
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: bytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
    }
}