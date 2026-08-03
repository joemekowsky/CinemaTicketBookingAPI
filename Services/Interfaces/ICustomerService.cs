using CinemaTicketBookingAPI.DTOs.Customer;
using CinemaTicketBookingAPI.Models;

namespace CinemaTicketBookingAPI.Services.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<CustomerDto> GetByIdAsync(int id);

    // Finds an existing guest customer by email, or creates a new one.
    // Used internally by booking creation — guests have no account/login.
    Task<Customer> GetOrCreateGuestAsync(string name, string email);
}