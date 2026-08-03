namespace CinemaTicketBookingAPI.Exceptions;

public class ShowTimeNotFoundException : Exception
{
    public ShowTimeNotFoundException(int id) : base($"ShowTime with id {id} was not found.") { }
}