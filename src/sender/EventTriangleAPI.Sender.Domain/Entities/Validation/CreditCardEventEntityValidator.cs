using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class CreditCardEventEntityValidator : AbstractValidator<CreditCardEventEntity>
{
    public CreditCardEventEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CardId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.HolderName).NotEmpty();
        RuleFor(x => x.CardNumber).NotEmpty();
    }
}