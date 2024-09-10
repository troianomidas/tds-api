namespace WebApi.Domain.Exceptions;

public class VerificationAttemptsException : BadRequestException
{
    public VerificationAttemptsException(string? email)
        : base($"Você '{email}' atingiu limite de tentativas. Por favor, tente novamente mais tarde")
    {
    }

    public string GetMessage() => "Você atingiu limite de tentativas. Por favor, tente novamente mais tarde";
}