using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class TransactionCreatedEventValidator : AbstractValidator<TransactionCreatedEvent>
{
    public TransactionCreatedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Id).NotEmpty();
    }
}