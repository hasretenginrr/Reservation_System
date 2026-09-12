# Multi-Client Reservation System

[English](#overview) | [Türkçe](#türkçe)

## Overview

A C# reservation project with a shared ASP.NET Core API, a Razor Pages web interface, a WPF desktop client, and a console client. It demonstrates database persistence, DTOs, service classes, input validation, and HTTP communication across multiple interfaces.

This is a development/learning project. Several authentication, error-handling, and client integration paths need correction before deployment.

## Solution Structure

| Project | Responsibility | Target |
| --- | --- | --- |
| `Reservation_System` | API controllers and Razor Pages in the same application | .NET 8 |
| `Reservation_SystemWPF` | WPF client with Views, ViewModels, ObservableObject, and RelayCommand | .NET 8 Windows |
| `Reservetion_System.Client` | Console login, signup, and reservation client | .NET 8 |

The original spelling of `Reservetion_System.Client` is retained. Within the web project, `Controllers`, `Services`, `DTO`, `Entities`, and `Data` separate responsibilities through folders rather than separate class-library projects.

## Technologies

- C# / ASP.NET Core / Razor Pages
- Entity Framework Core **9.0.8** with SQL Server
- FluentValidation **12.0.0**
- ASP.NET Core cookie authentication and claims in the web login flow
- WPF / XAML and an MVVM-style structure
- HttpClient and JSON for client/API communication
- HTML, CSS, JavaScript, and bundled Yummy-Red template assets
- EF Core migrations

The inspected implementation does not configure ASP.NET Identity or JWT issuance. The login API returns user information; the Razor Page creates the web authentication cookie.

## Features and Data

- User registration with email and phone validation and a duplicate email/phone check.
- Login based on username and phone number.
- Category listing.
- Reservation creation with user, category, date, and group size.
- Queries for all reservations or a given user's reservations.
- Reservation deletion endpoint, with known implementation issues below.
- Contact-message submission.
- Web login claims including user ID and role.
- Web reservation-list logic intended to distinguish administrators from normal users.
- WPF and console clients that call the same API.

Database entities: `Users`, `Categories`, `Reservations`, and `Messages`.

### Reservation Validation

The current validator requires:

- An existing category ID.
- A reservation date from today through the next month.
- **2–15 people**. The error message incorrectly mentions a maximum of 30.

## API Routes

These routes are present in the source; this table is not a claim that every path works correctly or is securely protected.

| Method | Route | Purpose |
| --- | --- | --- |
| POST | `/api/User` | Create a user |
| GET | `/api/User` | List users |
| POST | `/api/Login` | Look up a user using username and phone |
| POST | `/api/Logout` | Sign out the cookie session |
| GET | `/api/Category` | List categories |
| POST | `/api/Reservation` | Create a reservation |
| GET | `/api/Reservation` | List reservations |
| GET | `/api/Reservation/user/{userId}` | List reservations by user ID |
| DELETE | `/api/Reservation/{id}` | Delete a reservation |
| POST | `/api/Messages` | Submit a message |

## Local Setup

Use a disposable local SQL Server database and synthetic data. Do not expose the current API publicly.

1. Clone this repository and open `ReservationSystem.sln`.
2. Install the .NET 8 SDK. To run the full solution including WPF, use Windows and suitable Visual Studio desktop/web workloads.
3. Set `ConnectionStrings:DefaultConnection` for your local SQL Server instance in local development configuration. The checked-in configuration uses Windows authentication and the database name `ReservationDB`. Do not commit credentials.
4. Restore and build the web project:

   ```bash
   dotnet restore Reservation_System/Reservation_System.csproj
   dotnet build Reservation_System/Reservation_System.csproj
   ```

5. Review and apply the included migrations to the disposable database. In Visual Studio's Package Manager Console, using the included EF tools:

   ```powershell
   Update-Database -Project Reservation_System -StartupProject Reservation_System
   ```

   Migration execution was not verified during this documentation update. Resolve any model/migration mismatch before proceeding.
6. Populate the `Categories` table with test categories. No category-creation API is included; records in `StaticData.cs` are not a database seeding mechanism.
7. Ensure the ASP.NET Core development HTTPS certificate is trusted locally, then start the HTTPS profile:

   ```bash
   dotnet run --project Reservation_System/Reservation_System.csproj --launch-profile https
   ```

8. Open `https://localhost:7067`. The web login and the desktop/console clients reference that address.
9. Keep the API running and start the optional console client:

   ```bash
   dotnet run --project Reservetion_System.Client/Reservetion_System.Client.csproj
   ```

10. On Windows, start the optional WPF client:

    ```bash
    dotnet run --project Reservation_SystemWPF/Reservation_SystemWPF.csproj
    ```

These are source-derived setup instructions, not verified successful execution. See the integration limitations below.

## Known Issues and Security Boundaries

- Username plus phone number is not strong authentication. Use test identities only.
- The inspected user/reservation API controllers do not enforce authorization. Role-aware page logic does not protect the API. User IDs in requests need ownership validation.
- `LoginController` repeats `[HttpPost]` and dereferences the login result without first handling a failed lookup.
- Several controllers check whether a service result is null instead of checking `HasError`, so an HTTP 200 response can contain a failed operation.
- Deletion checks `result.Result` instead of the fetched reservation and continues after the error. Its controller returns `NotFound` when `HasError` is false.
- `GetReservations()` fetches records but does not assign them to its result. The separate async query methods do construct lists.
- The reservation-list Razor Page uses relative request URLs without setting HttpClient's BaseAddress.
- The WPF reservation ViewModel creates a new HttpClient and omits UserId without establishing an authenticated session. It also treats an HTTP success response as operation success without checking the result body.
- Session services are added, but no explicit distributed-cache implementation is registered in the inspected startup code; verify session startup and configure a cache if required.
- The CORS policy allows any origin, method, and header. Restrict it for deployment.
- Login input is logged by the web page. Avoid logging personal/authentication data.
- No automated test project or CI workflow was found.

## Türkçe

### Proje Hakkında

Ortak bir ASP.NET Core API'sine bağlanan Razor Pages web arayüzü, WPF masaüstü istemcisi ve konsol istemcisinden oluşan C# rezervasyon projesidir. DTO, servis, entity ve veri erişimi sorumlulukları web projesi içinde klasörlerle ayrılmıştır.

SQL Server, Entity Framework Core 9.0.8 ve FluentValidation 12.0.0 kullanır. Web ve konsol projeleri .NET 8, WPF projesi .NET 8 Windows hedefler.

### Özellikler

- Kullanıcı kaydı ve e-posta/telefon doğrulaması.
- Kullanıcı adı ve telefonla giriş.
- Kategori listeleme ve rezervasyon oluşturma.
- Tüm rezervasyonları veya kullanıcıya ait kayıtları sorgulama.
- Silme endpoint'i ve iletişim mesajı gönderimi.
- Web arayüzünde cookie ve claim tabanlı oturum.
- Aynı API ile iletişim kuran WPF ve konsol istemcileri.

Kod, ASP.NET Identity veya JWT üretimi yapılandırmaz. API kullanıcı bilgisini döndürür; web oturum çerezini Razor Page oluşturur. Rol bazlı ekran davranışı, tek başına API yetkilendirmesi sağlamaz.

### Kurulum

1. Depoyu klonlayıp `ReservationSystem.sln` dosyasını açın.
2. .NET 8 SDK ve SQL Server hazırlayın. WPF için Windows gerekir.
3. `ConnectionStrings:DefaultConnection` değerini kendi deneme veritabanınıza göre düzenleyin; parola gibi bilgileri Git'e eklemeyin.
4. Yukarıdaki restore/build komutlarını çalıştırın ve migration dosyalarını inceleyerek boş deneme veritabanına uygulayın.
5. `Categories` tablosuna örnek kayıtlar ekleyin. `StaticData.cs` veritabanını otomatik doldurmaz.
6. HTTPS geliştirme sertifikasını hazırlayıp web projesini `https` profiliyle başlatın.
7. `https://localhost:7067` adresini açın. API çalışırken konsol veya WPF istemcisini ayrıca başlatabilirsiniz.

### Mevcut Durum

Rezervasyon doğrulaması bugün ile gelecek ay arasındaki tarihleri ve **2–15 kişiyi** kabul eder; hata mesajındaki 30 sınırı kodla tutarlı değildir.

API yetkilendirmesi, başarısız giriş kontrolü, silme işlemi, HTTP hata yanıtları, rezervasyon listeleme sayfasının BaseAddress ayarı ve WPF kullanıcı/oturum aktarımı geliştirilmelidir. Kullanıcı adı ve telefonla giriş, üretim ortamı için yeterli güvenlik sağlamaz.

Bu güncellemede yalnızca README eklenmiştir. Uygulama kodu değiştirilmemiş; migration, derleme, istemciler ve testler çalıştırılmamıştır.
