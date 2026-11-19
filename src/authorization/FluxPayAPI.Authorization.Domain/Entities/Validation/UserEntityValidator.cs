using FluxPayAPI.Shared.Application.PredicateValidators;
using FluentValidation;

namespace FluxPayAPI.Authorization.Domain.Entities.Validation;

public class UserEntityValidator : AbstractValidator<UserEntity>
{
    public UserEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Email).Must(UserPredicates.ValidateEmail);
    }
}
