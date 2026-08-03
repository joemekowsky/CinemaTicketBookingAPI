namespace CinemaTicketBookingAPI.Exceptions;

public class CustomerNotFoundException : Exception
{
    public CustomerNotFoundException(int id) : base($"Customer with id {id} was not found.") { }
}