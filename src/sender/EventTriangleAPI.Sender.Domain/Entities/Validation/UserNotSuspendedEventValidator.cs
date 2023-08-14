using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class UserNotSuspendedEventValidator : AbstractValidator<UserNotSuspendedEvent>
{
    public UserNotSuspendedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RequesterId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}