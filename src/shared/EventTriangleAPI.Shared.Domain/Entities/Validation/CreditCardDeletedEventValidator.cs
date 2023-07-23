using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Entities.Validation;

public class CreditCardDeletedEventValidator : AbstractValidator<CreditCardDeletedEvent>
{
    public CreditCardDeletedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CardId).NotEmpty();
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}