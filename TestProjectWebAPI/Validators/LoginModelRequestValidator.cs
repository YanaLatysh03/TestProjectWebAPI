using DocumentService.Models.Request;
using FluentValidation;

namespace DocumentService.Validators
{
    public class LoginModelRequestValidator : AbstractValidator<LoginModelRequest>
    {
        public LoginModelRequestValidator()
        {
            RuleFor(model => model.Email).NotNull().EmailAddress();
            RuleFor(model => model.Password).NotNull();
        }
    }
}
