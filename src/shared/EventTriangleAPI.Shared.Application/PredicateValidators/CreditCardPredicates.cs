namespace EventTriangleAPI.Shared.Application.PredicateValidators;

public static class CreditCardPredicates
{
    public static bool CheckExpiration(string value)
    {
        if (value == null) return false;
        
        var intArray = value.Split('/');

        if (intArray.Any(intItem => intItem.Length != 2)) return false;
        
        if (intArray.Length != 2) return false;
        if (!int.TryParse(intArray[0], out var int1)) return false;
        if (!int.TryParse(intArray[1], out var int2)) return false;

        if (int1 is < 0 or > 12) return false;
        if (int2 is < 0 or > 99) return false;

        return true;
    }
}