using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class SupportTicketResolvedEventValidator : AbstractValidator<SupportTicketResolvedEvent>
{
    public SupportTicketResolvedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RequesterId).NotEmpty();
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.TicketJustification).NotEmpty().Length(1, 300);
        RuleFor(x => x.CreatedAt).NotEmpty();
    }     
}