using Microsoft.EntityFrameworkCore;
using PharmacyManagement.DTOs;
using PharmacyManagement.Models;
using PharmacyManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagement.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO> GetCustomerByIdAsync(int id);
        Task<CustomerDTO> CreateCustomerAsync(CreateCustomerDTO createDto);
        Task<bool> UpdateCustomerAsync(int id, CreateCustomerDTO updateDto);
        Task<bool> DeleteCustomerAsync(int id);
    }

    public class CustomerService : ICustomerService
    {
        private readonly PharmacyContext _context;

        public CustomerService(PharmacyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync()
        {
            var customers = await _context.Customers
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return customers.Select(c => new CustomerDTO
            {
                Id = c.Id,
                Name = c.Name,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Address = c.Address,
                City = c.City,
                District = c.District,
                Ward = c.Ward,
                DateOfBirth = c.DateOfBirth,
                Gender = c.Gender,
                TotalSpending = c.TotalSpending
            });
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int id)
        {
            var c = await _context.Customers.FindAsync(id);
            if (c == null || !c.IsActive) return null;

            return new CustomerDTO
            {
                Id = c.Id,
                Name = c.Name,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Address = c.Address,
                City = c.City,
                District = c.District,
                Ward = c.Ward,
                DateOfBirth = c.DateOfBirth,
                Gender = c.Gender,
                TotalSpending = c.TotalSpending
            };
        }

        public async Task<CustomerDTO> CreateCustomerAsync(CreateCustomerDTO createDto)
        {
            var customer = new Customer
            {
                Name = createDto.Name,
                PhoneNumber = createDto.PhoneNumber,
                Email = createDto.Email,
                Address = createDto.Address,
                City = createDto.City,
                District = createDto.District,
                Ward = createDto.Ward,
                DateOfBirth = createDto.DateOfBirth,
                Gender = createDto.Gender,
                TotalSpending = 0,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                IsActive = true
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return new CustomerDTO
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Address = customer.Address,
                City = customer.City,
                District = customer.District,
                Ward = customer.Ward,
                DateOfBirth = customer.DateOfBirth,
                Gender = customer.Gender,
                TotalSpending = customer.TotalSpending
            };
        }

        public async Task<bool> UpdateCustomerAsync(int id, CreateCustomerDTO updateDto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null || !customer.IsActive) return false;

            customer.Name = updateDto.Name;
            customer.PhoneNumber = updateDto.PhoneNumber;
            customer.Email = updateDto.Email;
            customer.Address = updateDto.Address;
            customer.City = updateDto.City;
            customer.District = updateDto.District;
            customer.Ward = updateDto.Ward;
            customer.DateOfBirth = updateDto.DateOfBirth;
            customer.Gender = updateDto.Gender;
            customer.LastModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            customer.IsActive = false; // Xóa mềm
            await _context.SaveChangesAsync();
            return true;
        }
    }
}