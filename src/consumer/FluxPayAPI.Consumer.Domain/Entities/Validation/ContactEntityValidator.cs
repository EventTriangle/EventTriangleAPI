using FluentValidation;

namespace FluxPayAPI.Consumer.Domain.Entities.Validation;

public class ContactEntityValidator : AbstractValidator<ContactEntity>
{
    public ContactEntityValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ContactId).NotEmpty();
    }
}