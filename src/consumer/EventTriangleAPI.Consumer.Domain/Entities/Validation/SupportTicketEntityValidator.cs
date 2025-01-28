using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities.Validation;

public class SupportTicketEntityValidator : AbstractValidator<SupportTicketEntity>
{
    public SupportTicketEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.TransactionId).NotEmpty();
        RuleFor(x => x.TicketReason).NotEmpty().Length(1, 300);
        RuleFor(x => x.TicketJustification).MaximumLength(300);
    }
}