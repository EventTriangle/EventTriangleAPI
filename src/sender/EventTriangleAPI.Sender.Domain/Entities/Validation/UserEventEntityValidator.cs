using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class UserEventEntityValidator : AbstractValidator<UserEventEntity>
{
    public UserEventEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}