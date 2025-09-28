namespace Reservation_System.DTO
{
    public class ReservationDto
    {
        public int? UserId { get; set; }
        public int CategoryId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PeopleCount { get; set; }

    }
}
