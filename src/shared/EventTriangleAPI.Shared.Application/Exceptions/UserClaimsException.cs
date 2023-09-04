namespace EventTriangleAPI.Shared.Application.Exceptions;

public class UserClaimsException : Exception
{
    public UserClaimsException(string message) : base(message) {}
}