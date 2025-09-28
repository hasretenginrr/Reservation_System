using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Reservation_System.Entities
{
    public class Categories
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Reservation> Reservations { get; set; }


        //public List<Reservation> Reservations { get; set; }= new List<Reservation>();

    }
}
