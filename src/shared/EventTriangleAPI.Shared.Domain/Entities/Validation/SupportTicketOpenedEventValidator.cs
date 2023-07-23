using FluentValidation;

namespace EventTriangleAPI.Shared.Domain.Entities.Validation;

public class SupportTicketOpenedEventValidator : AbstractValidator<SupportTicketOpenedEvent>
{
    public SupportTicketOpenedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.TicketReason).NotEmpty().Length(1, 300);
        RuleFor(x => x.CreatedAt).NotEmpty();
    }
}