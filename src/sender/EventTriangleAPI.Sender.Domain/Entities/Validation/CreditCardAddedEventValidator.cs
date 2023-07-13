using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class CreditCardAddedEventValidator : AbstractValidator<CreditCardAddedEvent>
{
    public CreditCardAddedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CardId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.HolderName).NotEmpty();
        RuleFor(x => x.CardNumber).Length(16);
        RuleFor(x => x.Cvv).Length(3);
        RuleFor(x => x.Expiration).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}