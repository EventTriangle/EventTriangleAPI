using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities.Validation;

public class CreditCardEntityValidator : AbstractValidator<CreditCardEntity>
{
    public CreditCardEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.HolderName).NotEmpty();
        RuleFor(x => x.CardNumber).NotEmpty();
        RuleFor(x => x.Cvv).NotEmpty();
    }
}