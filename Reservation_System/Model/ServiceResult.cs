namespace Reservation_System.Model
{
    public class ServiceResult
    {

        public object Result { get; set; }
        
        public string Message { get; set; }

        public bool HasError { get; set; }
    }
}
