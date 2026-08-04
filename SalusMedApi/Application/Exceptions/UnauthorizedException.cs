namespace SalusMedApi.CrossCutting.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message)
        : base(message) { }
}
