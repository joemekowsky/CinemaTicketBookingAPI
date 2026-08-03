using CinemaTicketBookingAPI.Data;
using CinemaTicketBookingAPI.Models;
using CinemaTicketBookingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingAPI.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CinemaDbContext _context;

    public CustomerRepository(CinemaDbContext context)
    {
        _context = context;
    }

    public IQueryable<Customer> GetQueryable() => _context.Customers.AsQueryable();

    public async Task<Customer?> GetByIdAsync(int id) =>
        await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Customer?> GetByEmailAsync(string email) =>
        await _context.Customers.FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());

    public async Task AddAsync(Customer customer) => await _context.Customers.AddAsync(customer);

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}