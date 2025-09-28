namespace Reservation_System.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }

        public string Role { get; set; } = "User";

        public ICollection<Reservation> Reservations { get; set; }


    }
}
