using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class ContactDeletedEventValidator : AbstractValidator<ContactDeletedEvent>
{
    public ContactDeletedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RequesterId).NotEmpty();
        RuleFor(x => x.ContactId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}