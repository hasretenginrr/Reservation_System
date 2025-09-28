using FluentValidation;
using Reservation_System.Data;
using Reservation_System.DTO;

namespace Reservation_System.Services
{
    public class LoginValidator :AbstractValidator<UserLoginDto>
    {
        private readonly AppDbContext _appDbContext;
    
        public LoginValidator(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

            RuleFor(x => x.UserName).NotEmpty().WithMessage("username required");
            RuleFor(x => x.Phone)
               .NotEmpty().WithMessage("phone number required")
               .Matches(@"^\d{10,15}$").WithMessage("Please enter a valid phone number.");
        }
    
    }
}
