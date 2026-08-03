using CinemaTicketBookingAPI.Models;

namespace CinemaTicketBookingAPI.Repositories.Interfaces;

public interface ICustomerRepository
{
    IQueryable<Customer> GetQueryable();
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer?> GetByEmailAsync(string email);
    Task AddAsync(Customer customer);
    Task<bool> SaveChangesAsync();
}