using System.Text.RegularExpressions;

namespace FluxPayAPI.Shared.Application.PredicateValidators;

public static partial class UserPredicates
{
    public static bool ValidateEmail(string email)
    {
        if (email == null) return false;

        return EmailPattern().IsMatch(email);
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.None, 1000)]
    private static partial Regex EmailPattern();
}