using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation_SystemWPF.Models
{
    public class ApiResponse<T>
    {
        public T Result { get; set; }
        public string Message { get; set; }
        public bool HasError { get; set; }
    }

}
