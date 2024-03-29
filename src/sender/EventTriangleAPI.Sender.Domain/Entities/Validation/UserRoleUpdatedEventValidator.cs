using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class UserRoleUpdatedEventValidator : AbstractValidator<UserRoleUpdatedEvent>
{
    public UserRoleUpdatedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RequesterId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}