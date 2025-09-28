using System.ComponentModel.DataAnnotations;

namespace Reservation_System.Entities
{
    public class Messages
    {
        [Key]
        public int MessageId { get; set; }
        public string Subject { get; set; }

        public string Message { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }           

    }
}
