namespace SalusMedApi.CrossCutting.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message)
        : base(message) { }
}
