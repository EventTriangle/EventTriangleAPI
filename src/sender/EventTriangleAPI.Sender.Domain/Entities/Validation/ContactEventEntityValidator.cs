using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class ContactEventEntityValidator : AbstractValidator<ContactEventEntity>
{
    public ContactEventEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ContactId).NotEmpty();
    }
}