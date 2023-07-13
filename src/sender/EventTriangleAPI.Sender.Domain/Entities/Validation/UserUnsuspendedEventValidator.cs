using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class UserUnsuspendedEventValidator : AbstractValidator<UserUnsuspendedEvent>
{
    public UserUnsuspendedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}