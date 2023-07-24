using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Events.Validation;

public class UserSuspendedEventValidator : AbstractValidator<UserSuspendedEvent>
{
    public UserSuspendedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}