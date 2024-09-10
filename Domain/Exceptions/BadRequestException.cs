namespace WebApi.Domain.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string msg)
        : base(msg)
    {
    }
}