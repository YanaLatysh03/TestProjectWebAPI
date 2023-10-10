using DocumentService.Database;
using DocumentService.Models.Request;
using FluentValidation;

namespace DocumentService.Validators
{
    public class ManageUserRequestValidator : AbstractValidator<ManageUserRequest>
    {
        private readonly ApplicationContext _context;

        public ManageUserRequestValidator(ApplicationContext context)
        {
            RuleFor(model => model.Name).NotEmpty().WithMessage("Name is empty");
            RuleFor(model => model.Email).NotEmpty().WithMessage("Email is empty")
                .EmailAddress().WithMessage("Email is not corrected")
                .Must(CheckUniqueEmail).WithMessage("Email is not unique");
            RuleFor(model => model.Age).GreaterThanOrEqualTo(0).WithMessage("Age is not corrected");
            RuleFor(model => model.Password).NotEmpty().WithMessage("Password is empty")
                .Must(CheckUniquePassword).WithMessage("Password is not unique");
            _context = context;
        }

        public virtual bool CheckUniqueEmail(string email)
        {
            return !_context.Users.Any(user => user.Email == email);
        }

        public virtual bool CheckUniquePassword(string password)
        {
            return !_context.Users.Any(user => user.Password == password);
        }
    }
}
