namespace SalusMedApi.CrossCutting.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message)
        : base(message) { }
}
