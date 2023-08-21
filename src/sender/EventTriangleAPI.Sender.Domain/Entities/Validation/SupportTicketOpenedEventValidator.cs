using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class SupportTicketOpenedEventValidator : AbstractValidator<SupportTicketOpenedEvent>
{
    public SupportTicketOpenedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RequesterId).NotEmpty();
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.TicketReason).NotEmpty().Length(1, 300);
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}