using DocumentService.Database;
using DocumentService.Validators;
using FluentValidation;

namespace WebAPI.Validators
{
    public class UpdateUserDataRequestValidator : ManageUserRequestValidator
    {
        private readonly ApplicationContext _context;

        public UpdateUserDataRequestValidator(ApplicationContext context) : base(context) 
        { 
            _context = context;
        }

        public override bool CheckUniqueEmail(string email)
        {
            return !_context.Users.Where(user => user.Email != email).Any(user => user.Email == email);
        }

        public override bool CheckUniquePassword(string password)
        {
            return !_context.Users.Where(user => user.Password != password).Any(user => user.Password == password);
        }
    }
}
