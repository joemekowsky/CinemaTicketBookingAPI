namespace CinemaTicketBookingAPI.Exceptions;

public class MovieAlreadyExistsException : Exception
{
    public MovieAlreadyExistsException(string name) : base($"A movie named '{name}' already exists.") { }
}