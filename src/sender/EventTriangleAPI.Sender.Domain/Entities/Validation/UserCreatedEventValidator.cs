using EventTriangleAPI.Shared.Application.PredicateValidators;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class UserCreatedEventValidator : AbstractValidator<UserCreatedEvent>
{
    public UserCreatedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Email).Must(UserPredicates.ValidateEmail);
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}