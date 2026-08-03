namespace CinemaTicketBookingAPI.DTOs.Auditorium;

public class AuditoriumDto
{
    public int Id { get; set; }
    public int RoomNumber { get; set; }
    public int Capacity { get; set; }
    public bool Available { get; set; }
}