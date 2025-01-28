using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class TransactionUserToUserCreatedEventValidator : AbstractValidator<TransactionUserToUserCreatedEvent>
{
    public TransactionUserToUserCreatedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RequesterId).NotEmpty();
        RuleFor(x => x.ToUserId).NotEmpty();
        RuleFor(x => x.Amount).Must(x => x > 0);
    }
}