


using Reservation_System.DTO;

public class ApiResponse<T>
{
    public T Result { get; set; }
    public string Message { get; set; }
    public bool HasError { get; set; }

    public List<ValidationError> Errors { get; set; } = new List<ValidationError>();

}
