namespace WebApi.Domain.Exceptions;

public class DataNotFoundException : Exception
{
    public DataNotFoundException(string? obj)
        : base($"Ops! Parece que o registro {obj} que você está procurando não existe.")
    {
    }
}