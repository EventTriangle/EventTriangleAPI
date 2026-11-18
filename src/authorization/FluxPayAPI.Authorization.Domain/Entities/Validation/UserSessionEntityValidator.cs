using FluentValidation;

namespace FluxPayAPI.Authorization.Domain.Entities.Validation;

public class UserSessionEntityValidator : AbstractValidator<UserSessionEntity>
{
    public UserSessionEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Value).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}