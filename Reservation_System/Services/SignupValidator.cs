using FluentValidation;
using Reservation_System.Data;
using Reservation_System.DTO;
using System.Linq;

namespace Reservation_System.Services
{
    public class SignupValidator : AbstractValidator<UserDto>
    {
        private readonly AppDbContext _appDbContext;
        public SignupValidator(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

            RuleFor(x => x.mail)
                .NotEmpty().WithMessage("Mail required!")
                .EmailAddress().WithMessage("Please enter a valid email adress!");
           
            RuleFor(x => x.phone)
            .NotEmpty().WithMessage("Phone required.")
            .Matches(@"^0\d+$").WithMessage("Please enter your number in the correct format and precede it with a 0.");
           

            RuleFor(x => x)
                .Must(dto => !IsUserAlreadyRegistered(dto))
                .WithMessage("This email or phone number is already registered.");
        }

        private bool IsUserAlreadyRegistered(UserDto dto)
        {
            return _appDbContext.Users.Any(u => u.Mail == dto.mail || u.Phone == dto.phone);
        }
    }
}
