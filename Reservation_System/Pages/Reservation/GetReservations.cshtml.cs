using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reservation_System.DTO;
using Reservation_System.Entities;
using System.Security.Claims;



namespace Reservation_System.Pages.GetReservations
{
    [Authorize]
    public class ReservationModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public List<ReservationDto> Reservations { get; set; }

        public ReservationModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }


        public async Task OnGetAsync()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (role == "admin")
            {
                // Admin tüm rezervasyonlarý çekebilir
                Reservations = await _httpClient.GetFromJsonAsync<List<ReservationDto>>("api/Reservation");
            }
            else
            {
                // Normal kullanýcý sadece kendi rezervasyonlarýný görebilir
                Reservations = await _httpClient.GetFromJsonAsync<List<ReservationDto>>($"api/Reservation/user/{userId}");
            }
        }
    }
}
