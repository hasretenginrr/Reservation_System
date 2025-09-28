using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Reservation_System.Data;
using Reservation_System.DTO;
using Reservation_System.Entities;
using Reservation_System.Model;
using System.Security.Cryptography.X509Certificates;

namespace Reservation_System.Services
{
    public interface IReservationService
    {
        Task<List<ReservationDto>> GetAllReservationsAsync();
        Task<List<ReservationDto>> GetReservationsByUserIdAsync(int userId);
    }
    public class ReservationServices : IReservationService
    {

        private readonly AppDbContext _context;
        private readonly IValidator<ReservationDto> _validator;
        private readonly UserService _userService;
        public ReservationServices(AppDbContext context, IValidator<ReservationDto> validator, UserService userService)
        {
            _context = context;
            _validator = validator;
            _userService = userService;
            
        }
        public ServiceResult SetReservation(ReservationDto reservationDto)
        {
            var result = new ServiceResult();
            var validationResult = _validator.Validate(reservationDto);
         
                //Console.WriteLine("\nMevcut Kategoriler:");
                //foreach (var c in StaticData.Category)
                //Console.WriteLine($"{c.CategoryId}- {c.CategoryName}");

                // Console.Write("Kategori No: ");
                //int categoryId = int.Parse(Console.ReadLine());     

                //Console.Write("Rezervasyon Tarihi (yyyy-MM-dd): ");
                //DateTime reservationDate = DateTime.Parse(Console.ReadLine());

                //Console.Write("Kişi Sayısı: ");
                //int peopleCount = int.Parse(Console.ReadLine());



                //ReservationDto reservationDto = new ReservationDto
                //{

                //    UserId = UserId,
                //    CategoryId = categoryId,
                //    ReservationDate = reservationDate,
                //    PeopleCount = peopleCount

                //};
                // Login olmuş kullanıcı id'sini token'dan al


                if (!validationResult.IsValid) {
                    result.HasError= true;
                    result.Result = validationResult.Errors.Select(x=> new { x.PropertyName, x.ErrorMessage }).ToList();
                    result.Message = string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage));

                return result;

                 }
            try { 
                var currentUserId = _userService.GetCurrentUserId();

                if (reservationDto.UserId == null)
                {
                    reservationDto.UserId = currentUserId;
                }

                if (!_context.Users.Any(u => u.UserId == reservationDto.UserId))
                {
                    result.HasError = true;
                    result.Message = "Geçersiz veya bulunmayan kullanıcı.";
                    return result;
                }


                var reservation = new Reservation
                {
                    UserId = (int)reservationDto.UserId,
                    CategoryId = reservationDto.CategoryId,
                    ReservationDate = reservationDto.ReservationDate,
                    PeopleCount = reservationDto.PeopleCount
                };

               
                _context.Reservations.Add(reservation);
                _context.SaveChanges();
                result.Message = "Rezervasyon başarıyla eklendi.";

                GetReservations();
                //WriteReservation();
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = "Hata oluştu: " + ex.Message;
            }

            return result;
        }
        //public bool SetReservation(object Reservation)

        //    /*int User_Id, int Category_Id, DateTime Reservation_Date,string Reservation_Time, int People_Count*/
        //{

        //    return true;
        //    //var user = StaticData.Users.FirstOrDefault(x => x.UserId == User_Id);
        //    //var Category = StaticData.Category.FirstOrDefault(x => x.CategoryId == Category_Id);

        //    //var newReservation = new Reservation
        //    //{

        //    //    ReservationId = StaticData.Reservations.Count + 1,
        //    //    ReservationDate = Reservation_Date,
        //    //    ReservationTime = Reservation_Time,
        //    //    CategoryId = Category_Id,
        //    //    PeopleCount = People_Count,
        //    //    UserId = User_Id
        //    //};

        //    //StaticData.Reservations.Add(newReservation);
        //}

        public ServiceResult GetReservations()
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var reservations = _context.Reservations.ToList();

                if (serviceResult.Result == null)
                {
                    serviceResult.Message = "Rezervasyon Bulunamadı";
                    serviceResult.HasError = true;
                    return serviceResult;
                   
                }

                return serviceResult;


            }
            catch (Exception ex)
            {
                serviceResult.HasError = true;
                serviceResult.Message = "Hatalı İşlem:" + ex.Message;
                return serviceResult;

            }
            

            //foreach (var r in StaticData.Reservations)
            //{
            //    var user = StaticData.Users.FirstOrDefault(u => u.UserId == r.UserId);
            //    var category = StaticData.Category.FirstOrDefault(c => c.CategoryId == r.CategoryId);

            //    string userName = user?.UserName ?? "Bilinmiyor";
            //    string categoryName = category?.CategoryName ?? "Bilinmiyor";

            //}

        }

        //public void WriteReservation()
        //{
        //    var allReservations = GetReservations();

        //    if (allReservations.Result is List<Reservation> reservations)
        //    {
        //        Console.WriteLine("\nTüm Rezervasyonlar:");
        //        foreach (var r in reservations)
        //        {
        //            var rUser = _context.Users.FirstOrDefault(u => u.UserId == r.UserId);
        //            var rCategory = _context.Categories.FirstOrDefault(c => c.CategoryId == r.CategoryId);
        //            Console.WriteLine($"İsim: {rUser?.UserName} - Kategori: {rCategory?.CategoryName} - Tarih: {r.ReservationDate.ToShortDateString()} - Kişi: {r.PeopleCount}");
        //        }
        //    }
        //}

        //public ServiceResult DeleteReservation(int Reservation_Id)
        //{
        //    ServiceResult serviceResult = new ServiceResult();

        //    try
        //    {
        //        var reservation = _context.Reservations.Where(x => x.ReservationId == Reservation_Id).FirstOrDefault();

        //       if (reservation != null)
        //        {
        //            _context.Reservations.Remove(reservation);
        //            serviceResult.Message = "İşlem Başarılı";

        //            return serviceResult;

        //        }
        //        else
        //        {

        //            serviceResult.Message = "Rezervasyon Bulunamadı";
        //            serviceResult.HasError = true;
        //            return serviceResult;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        serviceResult.HasError = true;
        //        serviceResult.Message = "Hatalı İşlem:" + ex.Message;
        //        return serviceResult;
        //    }
        //}


        public async Task<List<ReservationDto>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                .Select(r => new ReservationDto
                {
                   
                    UserId = r.UserId,
                    CategoryId = r.CategoryId,
                    ReservationDate = r.ReservationDate,
                    PeopleCount = r.PeopleCount
                }).ToListAsync();
        }

        public async Task<List<ReservationDto>> GetReservationsByUserIdAsync(int userId)
        {
            return await _context.Reservations
                .Where(r => r.UserId == userId)
                .Select(r => new ReservationDto
                {
                    
                    UserId = r.UserId,
                    CategoryId = r.CategoryId,
                    ReservationDate = r.ReservationDate,
                    PeopleCount = r.PeopleCount
                }).ToListAsync();
        }

        public ServiceResult DeleteReservation(int ResId)
        {
            var result = new ServiceResult();

            var reservation = _context.Reservations.FirstOrDefault(r => r.ReservationId == ResId);

            if (result.Result == null)
            {

                result.HasError = true;
                result.Message = "Reservation not found";

            }
            ;

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();

            return new ServiceResult
            {
                HasError = false,
                Message = "Reservation deleted successfully."
            };
        }
    }
}
