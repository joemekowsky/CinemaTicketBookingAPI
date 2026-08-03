using CinemaTicketBookingAPI.Models;

namespace CinemaTicketBookingAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool AvailableInCinema { get; set; }

        public ICollection<ShowTime> Shows { get; set; } = new List<ShowTime>();
    }
}