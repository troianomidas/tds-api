namespace WebApi.Domain.Exceptions;

public class VerificationCodeInvalidException : BadRequestException
{
    public VerificationCodeInvalidException(string? code)
        : base($"Código de verificação '{code}' inválido")
    {
    }

    public string GetMessage() => "Código de verificação inválido";
}