using FluentValidation;
using Reservation_System.Data;
using Reservation_System.DTO;
using Reservation_System.Entities;
using Reservation_System.Model;
using System.Globalization;
using System.Linq;

namespace Reservation_System.Services
{
    public class ReservationValidator : AbstractValidator<ReservationDto>
    {
        private readonly AppDbContext _appDbContext;
        public ReservationValidator(AppDbContext dbContext) {

            _appDbContext = dbContext;

            RuleFor(x => x.CategoryId).Must(id => dbContext.Categories.Any(c => c.CategoryId == id)).WithMessage("Please enter a valid character."); ;
            RuleFor(x => x.ReservationDate).Must(date => date.Date <= DateTime.Now.AddMonths(1) && date.Date >= DateTime.Now.Date).WithMessage("Please choose a date within the next month.");
            RuleFor(x => x.PeopleCount).InclusiveBetween(2, 15).WithMessage("Please choose a minimum of 2 and a maximum of 30 people!");




        }
    }
}
