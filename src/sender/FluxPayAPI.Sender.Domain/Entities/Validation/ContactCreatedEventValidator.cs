using FluentValidation;

namespace FluxPayAPI.Sender.Domain.Entities.Validation;

public class ContactCreatedEventValidator : AbstractValidator<ContactCreatedEvent>
{
    public ContactCreatedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RequesterId).NotEmpty();
        RuleFor(x => x.ContactId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}