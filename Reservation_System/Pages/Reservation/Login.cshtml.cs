using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reservation_System.DTO;
using Reservation_System.Services;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Security.Claims;
namespace Reservation_System.Pages.Reservation
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IValidator<UserLoginDto> _validator;
        private readonly UserService _userService;

        public LoginModel(IHttpClientFactory httpClientFactory, IValidator<UserLoginDto> validator,UserService userService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7067/");
            _validator = validator;
            _userService = userService;
        }


        [BindProperty]
        public UserLoginDto Input { get; set; }


        [TempData]
        public string ErrorMessage { get; set; }

        public void OnGet() { }
        
        public async Task<IActionResult> OnPostAsync() 
        {
            var validationResult = await _validator.ValidateAsync(Input);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                   
                    ModelState.AddModelError($"Input.{error.PropertyName}", error.ErrorMessage);
                }
                return Page();
            }

            try
            {
                // Gönderilecek DTO'yu JSON olarak konsola yazdýr
                var json = System.Text.Json.JsonSerializer.Serialize(Input);
                Console.WriteLine("Gönderilen DTO: " + json);

                     Input.Role ??= "User"; 

                var response = await _httpClient.PostAsJsonAsync("api/Login", Input);
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();

                if (!response.IsSuccessStatusCode || result.HasError)
                {
                    if (result?.Errors != null)
                    {
                        foreach (var err in result.Errors)
                        {
                            ModelState.AddModelError($"Input.{err.PropertyName}", err.ErrorMessage);
                        }
                    }
                    ModelState.AddModelError(string.Empty, result?.Message ?? "Giriþ baþarýsýz");
                    return Page();
                }

                var userId = result.Result.userId;
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.Result.userName),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Role, result.Result.role ?? "User")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToPage("/Reservation/Reservation");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluþtu: " + ex.Message);
                return Page();
            }

        }

    }

}
