using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Events.Validation;

public class TransactionRollBackedEventValidator : AbstractValidator<TransactionRollBackedEvent>
{
    public TransactionRollBackedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.TransactionId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}