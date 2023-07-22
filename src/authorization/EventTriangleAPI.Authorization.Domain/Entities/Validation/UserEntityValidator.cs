using FluentValidation;

namespace EventTriangleAPI.Authorization.Domain.Entities.Validation;

public class UserEntityValidator : AbstractValidator<UserEntity>
{
    public UserEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Username).NotEmpty();
    }
}