using Reservation_System.DTO;
using Reservation_System.Entities;
using Reservation_System.Model;

namespace Reservation_System.Services
{
    public class LoginServices
    {
        private readonly UserService _userService;
        public LoginServices(UserService userService)
        {
            _userService = userService;
        }
        public ServiceResult Login(UserLoginDto logindto)
        {
            ServiceResult serviceResult = new ServiceResult();
         //   UserLoginDto userLoginDto = new UserLoginDto();
            try
            {
                //Console.WriteLine("Kullanıcı adınızı girin: ");
                //string userName = Console.ReadLine();

                //Console.WriteLine("Telefon numaranızı giriniz: ");
                //string phone = Console.ReadLine();

                //userLoginDto.Phone = phone;
                //userLoginDto.UserName = userName;
                //var _user =  StaticData.Users.FirstOrDefault(x=>x.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) && x.Phone.Equals(phone));
              
                var userResult = _userService.GetUser(logindto);
                var user = (User)userResult.Result;

                if (user == null)
                {
                    //  Console.WriteLine("Kullanıcı adınız sistemde yok. Lütfen yeni kullanıcı oluşturun.");

                    //  userService.SetUser();

                    serviceResult.HasError = true;
                    serviceResult.Message = "Kullanıcı bulunamadı";
                }
                else {


                    //var userDto = new UserDto
                    //{
                    //    userId = user.UserId,
                    //    userName = user.UserName,
                    //    phone = user.Phone,
                    //    role = user.Role
                    //};

                    //serviceResult.Result = userDto;
                    serviceResult.Result = userResult.Result;
                    serviceResult.Message = "Kullanıcı bilgisi çekildi.";


                }
                return serviceResult;
            }
            catch(Exception ex) {

                serviceResult.HasError = true;
                serviceResult.Message = ex.Message;
                return serviceResult;
            }

        }


    }
}
