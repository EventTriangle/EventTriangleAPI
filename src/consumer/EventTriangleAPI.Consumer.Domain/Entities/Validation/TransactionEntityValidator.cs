using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities.Validation;

public class TransactionEntityValidator : AbstractValidator<TransactionEntity>
{
    public TransactionEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.From).NotEmpty();
        RuleFor(x => x.To).NotEmpty();
        RuleFor(x => x.FromWalletId).NotEmpty();
        RuleFor(x => x.ToWalletId).NotEmpty();
        RuleFor(x => x.Amount).NotEmpty();
    }
}