using FluentValidation;
using Reservation_System.Data;
using Reservation_System.DTO;

namespace Reservation_System.Services
{
    public class MessagesValidator : AbstractValidator<MessagesDto>
    {
        private readonly AppDbContext _context;

        public MessagesValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name).NotEmpty().WithMessage("lastname required");
            RuleFor(x => x.Subject).MaximumLength(50).WithMessage("maximum fifty characters").NotEmpty().WithMessage("please enter subject");
            RuleFor(x => x.Message).MaximumLength(500).WithMessage("To Long!").NotEmpty().WithMessage("please enter message!");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Enter in e-mail format");


        }
    }
}
