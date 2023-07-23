using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Entities.Validation;

public class UserRoleUpdatedEventValidator : AbstractValidator<UserRoleUpdatedEvent>
{
    public UserRoleUpdatedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}