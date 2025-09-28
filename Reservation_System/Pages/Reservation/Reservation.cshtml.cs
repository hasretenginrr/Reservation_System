using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reservation_System.Data;
using Reservation_System.DTO;
using Reservation_System.Entities;
using Reservation_System.Services;

namespace Reservation_System.Pages.Reservation
{
    [Authorize] // Kullanýcý login deðilse otomatik login sayfasýna yönlendir
    public class CreateReservationModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserService _userService;
        

        public CreateReservationModel(AppDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

    

        
        public int UserId { get; set; }
        public List<Categories> Categories { get; set; }

        public void OnGet()
        {
            UserId = _userService.GetCurrentUserId(); 
            Categories = _context.Categories.ToList(); 
        }
    }
}
