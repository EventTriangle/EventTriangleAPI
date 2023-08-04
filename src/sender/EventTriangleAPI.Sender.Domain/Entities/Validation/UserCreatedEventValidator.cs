using System.Text.RegularExpressions;
using FluentValidation;

namespace EventTriangleAPI.Sender.Domain.Entities.Validation;

public class UserCreatedEventValidator : AbstractValidator<UserCreatedEvent>
{
    public UserCreatedEventValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Email).Must(ValidateEmail);
        RuleFor(x => x.CreatedAt).NotEmpty();
    }

    private bool ValidateEmail(string email)
    {
        if (email == null) return false;

        return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    }
}