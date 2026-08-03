using CinemaTicketBookingAPI.Enums;

namespace CinemaTicketBookingAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int ShowTimeId { get; set; }
        public ShowTime ShowTime { get; set; }
    }
}