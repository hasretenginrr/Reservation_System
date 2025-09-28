using FluentValidation;
using Microsoft.AspNetCore.Http;
using Reservation_System.Data;
using Reservation_System.DTO;
using Reservation_System.Entities;
using Reservation_System.Model;
using System.Numerics;
using System.Security.Claims;

namespace Reservation_System.Services
{
    public class UserService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<UserDto> _validator;
        public UserService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, IValidator<UserDto> validator)
        {
            _appDbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;

        }
        public ServiceResult SetUser(UserDto userDto)
        {
            ServiceResult result = new ServiceResult();
            var validationResult= _validator.Validate(userDto);

            if (!validationResult.IsValid)
            {
                result.HasError = true;
                result.Result = validationResult.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }).ToList();
                result.Message = string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage));

                return result;

            }
            try
            {
                //Console.Write("Kullacı Adı: ");
                //string name = Console.ReadLine();

                //Console.Write("Telefon: ");
                //string phone = Console.ReadLine();

                //Console.Write("Mail: ");
                //string mail = Console.ReadLine();

                User user = new User
                {
                    UserName = userDto.userName,
                    Phone = userDto.phone,
                    Mail = userDto.mail,
                };

               
                _appDbContext.Users.Add(user);
                _appDbContext.SaveChanges();

                result.Message = "Kullanıcı eklendi.";

               // Console.WriteLine("Merhaba" + $"{user.UserName}\r" + "artık rezervasyon yapabilirsiniz.");

              //  reservationServices.SetReservation(user.UserId);
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = "Hata!" + ex.InnerException.Message;
            }
            return result;
        }
        public ServiceResult GetUsers()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Result = _appDbContext.Users;
                if (serviceResult.Result == null)
                {
                    serviceResult.HasError = true;
                    serviceResult.Message = "Kullanıcı bulunamadı";
                    return serviceResult;

                }
                return serviceResult;
            }
            catch (Exception ex)
            {
                serviceResult.HasError = true;
                serviceResult.Message = ex.Message;
                return serviceResult;
            }
        }


        public ServiceResult GetUser(UserLoginDto logindto) 
        {

            ServiceResult serviceResult = new ServiceResult();

            // var user = _appDbContext.Users.Where(x => x.UserName.Equals(logindto.UserName, StringComparison.OrdinalIgnoreCase) && x.Phone.Equals(logindto.Phone));
            // StringComparisonu desteklemiyormuş ef. O yüzden sorguyu değiştirdim. 

            var user = _appDbContext.Users.FirstOrDefault(x =>x.UserName.ToLower() == logindto.UserName.ToLower() && x.Phone == logindto.Phone);



            serviceResult.Result = user;
            


            return serviceResult;
        }

        public int GetCurrentUserId()
        {
            var claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null && int.TryParse(claim.Value, out int userId))
            {
                return userId;
            }
            return 0; // Login değilse veya userId bulunamazsa
        }
    }
}
