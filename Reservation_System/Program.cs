using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Reservation_System.Data;
using Reservation_System.DTO;
using Reservation_System.Entities;
using Reservation_System.Model;
using Reservation_System.Pages.Reservation;
using Reservation_System.Services;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();


builder.Services.AddRazorPages();

builder.Services.AddControllers();

builder.Services.AddScoped<IValidator<ReservationDto>, ReservationValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ReservationValidator>
    (includeInternalTypes: true);

builder.Services.AddRazorPages();
builder.Services.AddScoped<IValidator<UserLoginDto>, LoginValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>
    (includeInternalTypes: true);

builder.Services.AddScoped<IValidator<MessagesDto>, MessagesValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MessagesValidator>
    (includeInternalTypes: true);


//builder.Services.AddSingleton<LoginServices>();  migration yaparken hata aldým. (database aktarýrken) bunlarý singleton kullanamam 
//builder.Services.AddSingleton<UserService>();
//builder.Services.AddSingleton<ReservationServices>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<LoginServices>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ReservationServices>();
builder.Services.AddScoped<MessagesServices>();
builder.Services.AddSession();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Reservation/Login";
        options.AccessDeniedPath = "/Reservation/AccessDenied";
    });

builder.Services.AddAuthorization();

var app = builder.Build();
app.MapControllers();



//LoginServices loginServices = new LoginServices();

//loginServices.Login(logindto);



//var reservationService = new ReservationServices();
//var userService = new UserService();

//while (true)
//{

//    AddRes(reservationService, userService);

//    Console.WriteLine("\n Yeni rez. için 1, çýkmak için herhangi bir tuþa basýn.");
//        var input = Console.ReadLine();
//    if (input != "1") 
//        break;

//}
// void AddRes(ReservationServices reservationServices, UserService userService) { 
//        Console.Write("Kullanýcý Adý: ");
//        string userName = Console.ReadLine();

//        var user = StaticData.Users.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)); //büyük küçük harf duyarsýzlýðý için 


//        if (user == null)
//        {
//            Console.WriteLine("Yeni kullanýcý oluþturuluyor...");
//            Console.Write("Telefon: ");
//            string phone = Console.ReadLine();

//            Console.Write("Mail: ");
//            string mail = Console.ReadLine();

//            user = new User
//            {
//                UserName = userName,
//                Phone = phone,
//                Mail = mail
//            };

//            userService.SetUser(user); // Bu iþlem user.UserId'yi atýyor / userService de id için
//        }


//        Console.WriteLine("\nMevcut Kategoriler:");
//        foreach (var c in StaticData.Category)
//            Console.WriteLine($"{c.CategoryId}- {c.CategoryName}");

//        Console.Write("Kategori No: ");
//        int categoryId = int.Parse(Console.ReadLine());


//        Console.Write("Rezervasyon Tarihi (yyyy-MM-dd): ");
//        DateTime reservationDate = DateTime.Parse(Console.ReadLine());

//        Console.Write("Kiþi Sayýsý: ");
//        int peopleCount = int.Parse(Console.ReadLine());


//        var reservation = new Reservation
//        {
//            UserId = user.UserId,
//            CategoryId = categoryId,
//            ReservationDate = reservationDate,
//            PeopleCount = peopleCount
//        };

//        var result = reservationService.SetReservation(reservation);

//            if (!result.HasError)
//            {
//                var allReservations = reservationService.GetReservations();
//                if (allReservations.Result is List<Reservation> reservations)
//                {
//                    Console.WriteLine("\nTüm Rezervasyonlar:");
//                    foreach (var r in reservations)
//                    {
//                        var rUser = StaticData.Users.FirstOrDefault(u => u.UserId == r.UserId);
//                        var rCategory = StaticData.Category.FirstOrDefault(c => c.CategoryId == r.CategoryId);
//                        Console.WriteLine($"Ýsim: {rUser?.UserName} - Kategori: {rCategory?.CategoryName} - Tarih: {r.ReservationDate.ToShortDateString()} - Kiþi: {r.PeopleCount}");
//                    }


//                }
//            }
//}

//---------------------------------OLD--------------------------------------

//var reservationService = new ReservationServices();
//var userService = new UserService();
//Console.Write("Kullanýcý Adý: ");
//string userName = Console.ReadLine();


//var user = StaticData.Users.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
//if (user == null)
//{
//    Console.WriteLine("Kullanýcý bulunamadý! Rezervasyon için yeni kullanýcý oluþturun.\n");

//    Console.Write("Telefon: ");

//    string phone= Console.ReadLine();

//    Console.Write("Mail: ");

//    string mail = Console.ReadLine();

//    var newuser = new User
//    {

//        Mail = mail,
//        Phone = phone,
//        UserName = userName

//    };

//    ServiceResult result2 = userService.SetUser(newuser);

//}

//Console.WriteLine("\nMevcut Kategoriler:");
//foreach (var c in StaticData.Category)
//{
//    Console.WriteLine($"{c.CategoryId}- {c.CategoryName}");
//}


//Console.Write("Kategori No: ");
//int categoryId = int.Parse(Console.ReadLine());


//if (categoryId == null)
//{
//    Console.WriteLine("Kategori bulunamadý!");
//    return;
//}


//Console.Write("Rezervasyon Tarihi: ");
//DateTime reservationDate = DateTime.Parse(Console.ReadLine());


//Console.Write("Kiþi Sayýsý: ");
//int peopleCount = int.Parse(Console.ReadLine());


//var reservation = new Reservation
//{
//    UserId = user.UserId,
//    CategoryId = categoryId,
//    ReservationDate = reservationDate,
//    PeopleCount = peopleCount
//};


//    ServiceResult result = reservationService.SetReservation(reservation);


//if (!result.HasError)
//{
//    ServiceResult resResult = reservationService.GetReservations();

//    if (!resResult.HasError && resResult.Result is List<Reservation> reservations)
//    {
//        foreach (var r in reservations)
//        {
//            var user_Name = StaticData.Users.FirstOrDefault(u => u.UserId == r.UserId).UserName;
//            var category_Name = StaticData.Category.FirstOrDefault(c => c.CategoryId == r.CategoryId).CategoryName;

//            Console.WriteLine($"Ýsim: {user_Name} Kategori: {category_Name} Tarih: {r.ReservationDate} Kiþi Sayýsý: {r.PeopleCount}");
//        }
//    }
//}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication(); //bunlarýn sýrasý yüzünden sýkýntý yaþadým. Bunlarýn sýrasýný karýþtýrma!!!
app.UseAuthorization(); //ilk authentication

app.MapRazorPages();

app.Run();
