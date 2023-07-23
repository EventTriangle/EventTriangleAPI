using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Entities.Validation;

public class UserNotSuspendedEventValidator : AbstractValidator<UserNotSuspendedEvent>
{
    public UserNotSuspendedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}