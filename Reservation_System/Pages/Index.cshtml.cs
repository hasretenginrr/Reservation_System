using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Reservation_System.Pages
{
  //  [Authorize] burasý kalýrsa sayfa korunmaya alýnýr. yani giriþ yapmadan eriþilemez. O yüzden kaldýrdýk. 
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
