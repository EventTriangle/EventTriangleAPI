using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Entities.Validation;

public class CreditCardAddedEventValidator : AbstractValidator<CreditCardAddedEvent>
{
    public CreditCardAddedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CardId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.HolderName).NotEmpty();
        RuleFor(x => x.CardNumber).NotEmpty().Length(16);
        RuleFor(x => x.Cvv).NotEmpty().Length(3);
        RuleFor(x => x.Expiration).Must(CheckExpiration);
        RuleFor(x => x.CreatedAt).NotEmpty();
    }

    private bool CheckExpiration(string value)
    {
        if (value == null) return false;
        
        var intArray = value.Split('/');

        if (intArray.Any(intItem => intItem.Length != 2)) return false;
        
        if (intArray.Length != 2) return false;
        if (!int.TryParse(intArray[0], out var int1)) return false;
        if (!int.TryParse(intArray[1], out var int2)) return false;

        if (int1 is < 0 or > 31) return false;
        if (int2 is < 0 or > 12) return false;

        return true;
    }
}