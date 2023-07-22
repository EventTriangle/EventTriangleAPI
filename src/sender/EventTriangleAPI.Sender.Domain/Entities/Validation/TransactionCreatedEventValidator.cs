using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class TransactionCreatedEventValidator : AbstractValidator<TransactionCreatedEvent>
{
    public TransactionCreatedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.From).NotEmpty();
        RuleFor(x => x.To).NotEmpty();
        RuleFor(x => x.Amount).Must(x => x > 0);
        RuleFor(x => x.TransactionType).IsInEnum();
    }
}