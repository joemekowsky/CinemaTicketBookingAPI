using System.ComponentModel.DataAnnotations;

namespace CinemaTicketBookingAPI.DTOs.Auditorium;

public class UpdateAuditoriumDto
{
    [Required]
    public int RoomNumber { get; set; }

    [Range(1, 2000)]
    public int Capacity { get; set; }

    public bool Available { get; set; }
}