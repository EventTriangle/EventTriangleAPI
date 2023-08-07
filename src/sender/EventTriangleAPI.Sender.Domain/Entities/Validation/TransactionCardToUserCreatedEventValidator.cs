using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class TransactionCardToUserCreatedEventValidator : AbstractValidator<TransactionCardToUserCreatedEvent>
{
    public TransactionCardToUserCreatedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CreditCardId).NotEmpty();
        RuleFor(x => x.ToUserId).NotEmpty();
        RuleFor(x => x.Amount).Must(x => x > 0);
    }
}