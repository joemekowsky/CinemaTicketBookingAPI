namespace CinemaTicketBookingAPI.Models
{
    public class ShowTime
    {
        public int Id { get; set; }
        public DateTime ShowTimeDate { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int AuditoriumId { get; set; }
        public Auditorium Auditorium { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}