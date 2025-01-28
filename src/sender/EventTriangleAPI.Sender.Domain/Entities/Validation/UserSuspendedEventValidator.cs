using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class UserSuspendedEventValidator : AbstractValidator<UserSuspendedEvent>
{
    public UserSuspendedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RequesterId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}