using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class TransactionRollBackedEventValidator : AbstractValidator<TransactionRollBackedEvent>
{
    public TransactionRollBackedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RequesterId).NotEmpty();
        RuleFor(x => x.TransactionId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}