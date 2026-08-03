namespace CinemaTicketBookingAPI.Exceptions;

public class BookingNotFoundException : Exception
{
    public BookingNotFoundException(int id) : base($"Booking with id {id} was not found.") { }
}