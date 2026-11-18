using FluentValidation;

namespace FluxPayAPI.Sender.Domain.Entities.Validation;

public class TransactionCardToUserCreatedEventValidator : AbstractValidator<TransactionCardToUserCreatedEvent>
{
    public TransactionCardToUserCreatedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CreditCardId).NotEmpty();
        RuleFor(x => x.RequesterId).NotEmpty();
        RuleFor(x => x.Amount).Must(x => x > 0);
    }
}