using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Events.Validation;

public class ContactCreatedEventValidator : AbstractValidator<ContactCreatedEvent>
{
    public ContactCreatedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ContactId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}