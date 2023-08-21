using EventTriangleAPI.Shared.Application.PredicateValidators;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class CreditCardAddedEventValidator : AbstractValidator<CreditCardAddedEvent>
{
    public CreditCardAddedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RequesterId).NotEmpty();
        RuleFor(x => x.HolderName).NotEmpty();
        RuleFor(x => x.CardNumber).NotEmpty().Length(16);
        RuleFor(x => x.Cvv).NotEmpty().Length(3);
        RuleFor(x => x.Expiration).Must(CreditCardPredicates.CheckExpiration);
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}