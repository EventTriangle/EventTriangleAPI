using System.Text.RegularExpressions;
using FluentValidation;

namespace EventTriangleAPI.Authorization.Domain.Entities.Validation;

public class UserEntityValidator : AbstractValidator<UserEntity>
{
    public UserEntityValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Email).Must(ValidateEmail);
    }
    
    private bool ValidateEmail(string email)
    {
        if (email == null) return false;

        return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    }
}