using System.Text.RegularExpressions;

namespace EventTriangleAPI.Shared.Application.PredicateValidators;

public static class UserPredicates
{
    public static bool ValidateEmail(string email)
    {
        if (email == null) return false;

        return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    }
}