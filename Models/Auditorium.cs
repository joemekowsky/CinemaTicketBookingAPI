namespace CinemaTicketBookingAPI.Models
{
    public class Auditorium
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public int Capacity { get; set; }
        public bool Available { get; set; }

        public ICollection<ShowTime> Shows { get; set; } = new List<ShowTime>();
    }
}