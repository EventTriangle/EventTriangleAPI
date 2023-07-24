using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Events.Validation;

public class UserCreatedEventValidator : AbstractValidator<UserCreatedEvent>
{
    public UserCreatedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}