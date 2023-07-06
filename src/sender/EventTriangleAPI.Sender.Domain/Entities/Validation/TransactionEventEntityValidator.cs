using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class TransactionEventEntityValidator : AbstractValidator<TransactionEventEntity>
{
    public TransactionEventEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.From).NotEmpty();
        RuleFor(x => x.To).NotEmpty();
        RuleFor(x => x.Amount).NotEmpty();
    }
}