using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities.Validation;

public class TransactionEntityValidator : AbstractValidator<TransactionEntity>
{
    public TransactionEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FromUserId).NotEmpty();
        RuleFor(x => x.ToUserId).NotEmpty();
        RuleFor(x => x.Amount).Must(x => x > 0);
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}