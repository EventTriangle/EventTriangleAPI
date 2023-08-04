using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities.Validation;

public class UserEntityValidator : AbstractValidator<UserEntity>
{
    public UserEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.WalletId).NotEmpty();
    }
}