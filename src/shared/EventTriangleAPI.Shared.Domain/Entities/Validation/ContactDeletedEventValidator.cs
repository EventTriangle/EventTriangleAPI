using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Entities.Validation;

public class ContactDeletedEventValidator : AbstractValidator<ContactDeletedEvent>
{
    public ContactDeletedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ContactId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}