using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities.Validation;

public class WalletEntityValidator : AbstractValidator<WalletEntity>
{
    public WalletEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}