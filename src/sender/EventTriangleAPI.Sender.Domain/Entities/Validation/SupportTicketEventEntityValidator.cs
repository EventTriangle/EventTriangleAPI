using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class SupportTicketEventEntityValidator : AbstractValidator<SupportTicketEventEntity>
{
    public SupportTicketEventEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.TicketReason).NotEmpty();
    }
}