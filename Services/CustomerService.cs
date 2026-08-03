using CinemaTicketBookingAPI.DTOs.Customer;
using CinemaTicketBookingAPI.Exceptions;
using CinemaTicketBookingAPI.Models;
using CinemaTicketBookingAPI.Repositories.Interfaces;
using CinemaTicketBookingAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBookingAPI.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync() =>
        await _customerRepository.GetQueryable()
            .Select(c => ToDto(c))
            .ToListAsync();

    public async Task<CustomerDto> GetByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id)
            ?? throw new CustomerNotFoundException(id);
        return ToDto(customer);
    }

    public async Task<Customer> GetOrCreateGuestAsync(string name, string email)
    {
        var existing = await _customerRepository.GetByEmailAsync(email);
        if (existing is not null)
        {
            return existing;
        }

        var customer = new Customer { Name = name, Email = email };
        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveChangesAsync();

        return customer;
    }

    private static CustomerDto ToDto(Customer customer) => new()
    {
        Id = customer.Id,
        Name = customer.Name,
        Email = customer.Email
    };
}