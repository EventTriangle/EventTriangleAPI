using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class CreditCardDeletedEventValidator : AbstractValidator<CreditCardDeletedEvent>
{
    public CreditCardDeletedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RequesterId).NotEmpty();
        RuleFor(x => x.CardId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}