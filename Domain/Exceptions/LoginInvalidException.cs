namespace WebApi.Domain.Exceptions;

public class LoginInvalidException : BadRequestException
{
    public LoginInvalidException(string? email)
        : base($"Senha incorreta para o email '{email}'. Tente novamente ou redefina sua senha.")
    {
    }

    public string GetMessage() => "Senha incorreta para o e-mail. Tente novamente ou redefina sua senha.";
}