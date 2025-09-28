namespace Reservation_System.Entities
{
    public  class Reservation
    {
        public  int ReservationId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PeopleCount { get; set; }
        public  int UserId { get; set; }
        public User User { get; set; }
        public  int CategoryId { get; set; }
        public Categories Category { get; set; }

        public ReservationStatus Status { get; set; }


    }
    public enum ReservationStatus
    {
        Pending,    //kullanıcı oluturuldu
        Approved,   //Admin onayladı
        Cancelled //Admin iptal etti 

    }
}
