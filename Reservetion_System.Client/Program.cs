using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Reservation_System.DTO;

namespace ReservationSystem.ConsoleClient
{
    public class Program
    {
        private static int? _loggedInUserId = null;
        private static string _loggedInUserName = null;

        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7067/api/")
        };

        public static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n=== Rezervasyon Sistemi ===");
                Console.WriteLine("1 - Login");
                Console.WriteLine("2 - Yeni Kullanıcı Ekle");
                Console.WriteLine("3 - Yeni Rezervasyon Ekle");
                Console.WriteLine("0 - Çıkış");
                Console.Write("Seçiminiz: ");
                var secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        await Login();
                        break;
                    case "2":
                        await YeniKullanici();
                        break;
                    case "3":
                        await YeniRezervasyon();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim!");
                        break;
                }
            }
        }

        private static async Task Login()
        {
            Console.Write("Kullanıcı Adı: ");
            var username = Console.ReadLine();

            Console.Write("Telefon: ");
            var phone = Console.ReadLine();

            var loginDto = new UserLoginDto
            {
                UserName = username,
                Phone = phone
            };

            var response = await client.PostAsJsonAsync("Login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var userJson = await response.Content.ReadAsStringAsync();

                // apiden dönen veri result içinde olduğu için o veriyi işlemenin en pratik yolu ApiResponse adında bir model daha oluşturup içine girmekti. Böylellikle artık userId değerini çekebildik. 
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<UserDto>>(userJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (apiResponse?.Result != null)
                {
                    _loggedInUserId = apiResponse.Result.userId;
                    _loggedInUserName = apiResponse.Result.userName;
                    Console.WriteLine($"Hoşgeldiniz, {_loggedInUserName}!");
                }
                else
                {
                    Console.WriteLine("Login başarısız: Kullanıcı bilgisi alınamadı.");
                }
            }
            else
            {
                Console.WriteLine("Login başarısız: " + await response.Content.ReadAsStringAsync());
            }
        }

        private static async Task YeniKullanici()
        {
            Console.Write("Kullanıcı Adı: ");
            var username = Console.ReadLine();

            Console.Write("Telefon: ");
            var phone = Console.ReadLine();

            Console.Write("Mail: ");
            var mail = Console.ReadLine();

            var userDto = new UserDto
            {
                userName = username,
                phone = phone,
                mail = mail
            };

            var response = await client.PostAsJsonAsync("User", userDto);

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Kullanıcı: " + result);

            await Login();
        }

        private static async Task YeniRezervasyon()
        {
            if (_loggedInUserId == null)
            {
                Console.WriteLine("Önce giriş yapmalısınız.");
                return;
            }

            Console.Write("Kategori ID: ");
            var categoryId = Console.ReadLine();

            Console.Write("Rezervasyon Tarihi (yyyy-MM-dd): ");
            var date = Console.ReadLine();

            Console.Write("İnsan Sayısı: ");
            var peopleCount = Console.ReadLine();

            var rezervationDto = new
            {
                UserId = _loggedInUserId,
                CategoryId = int.Parse(categoryId),
                ReservationDate = date,
                PeopleCount = int.Parse(peopleCount)
            };

            var response = await client.PostAsJsonAsync("Reservation", rezervationDto);

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Rezervasyon: " + result);
        }
    }
}
