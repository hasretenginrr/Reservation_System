using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation_SystemWPF.Models
{
    internal class ReservationDto
    {
        public int? UserId { get; set; }
        public int CategoryId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int PeopleCount { get; set; }
    }
}
