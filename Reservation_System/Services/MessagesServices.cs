using FluentValidation;
using Reservation_System.Data;
using Reservation_System.DTO;
using Reservation_System.Entities;
using Reservation_System.Model;

namespace Reservation_System.Services
{
    public class MessagesServices
    {
        private readonly AppDbContext _context;
        private readonly IValidator<MessagesDto> _validator;

        public MessagesServices(AppDbContext context, IValidator<MessagesDto> validator)
        {
            _context = context;
            _validator = validator;

        }

        public ServiceResult SetMessages(MessagesDto messagesDto)
        {
            var result = new ServiceResult();
            var validationResult = _validator.Validate(messagesDto);

            if (!validationResult.IsValid)
            {
                result.HasError = true;
                result.Result = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                result.Message = string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage));

                return result;
            }
            try
            {
                var Messages = new Messages
                {
                    Name = messagesDto.Name,
                    Subject = messagesDto.Subject,
                    Message = messagesDto.Message,
                    Email = messagesDto.Email,

                };
                _context.Messages.Add(Messages);
                _context.SaveChanges();
                result.Message = "Mesaj başarıyla alındı";
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = "Hata oluştu!"+ ex.Message;

            }
            return result;            
       } 
    }
}
