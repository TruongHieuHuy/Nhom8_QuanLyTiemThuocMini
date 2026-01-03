using PharmacyManagement.DTOs;
using PharmacyManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagement.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO> GetCustomerByIdAsync(int id);
        Task<CustomerDetailDTO> GetCustomerDetailAsync(int id);
        Task<IEnumerable<CustomerDTO>> SearchCustomersAsync(string searchTerm);
        Task<CustomerDTO> CreateCustomerAsync(CreateCustomerDTO createDto);
        Task<CustomerDTO> UpdateCustomerAsync(UpdateCustomerDTO updateDto);
        Task<bool> DeleteCustomerAsync(int id);
    }

    public class CustomerService : ICustomerService
    {
        private readonly PharmacyManagement.Data.PharmacyContext _context;

        public CustomerService(PharmacyManagement.Data.PharmacyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync()
        {
            var customers = await System.Threading.Tasks.Task.Run(() =>
                _context.Customers.Where(c => c.IsActive).ToList()
            );
            return customers.Select(c => MapToDTO(c)).ToList();
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int id)
        {
            var customer = await System.Threading.Tasks.Task.Run(() =>
                _context.Customers.FirstOrDefault(c => c.Id == id)
            );
            return customer != null ? MapToDTO(customer) : null;
        }

        public async Task<CustomerDetailDTO> GetCustomerDetailAsync(int id)
        {
            var customer = await System.Threading.Tasks.Task.Run(() =>
                _context.Customers
                    .FirstOrDefault(c => c.Id == id)
            );

            if (customer == null)
                return null;

            return new CustomerDetailDTO
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                FullAddress = $"{customer.Ward}, {customer.District}, {customer.City}",
                TotalSpending = customer.TotalSpending,
                Orders = customer.Orders?.Select(o => new OrderDTO
                {
                    Id = o.Id,
                    OrderCode = o.OrderCode,
                    OrderDate = o.OrderDate,
                    Total = o.Total,
                    OrderStatus = o.OrderStatus
                }).ToList()
            };
        }

        public async Task<IEnumerable<CustomerDTO>> SearchCustomersAsync(string searchTerm)
        {
            var customers = await System.Threading.Tasks.Task.Run(() =>
                _context.Customers.Where(c => c.IsActive && (
                    c.Name.Contains(searchTerm) ||
                    c.PhoneNumber.Contains(searchTerm) ||
                    c.Email.Contains(searchTerm)
                )).ToList()
            );
            return customers.Select(c => MapToDTO(c)).ToList();
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
                CreatedDate = System.DateTime.Now,
                LastModifiedDate = System.DateTime.Now,
                IsActive = true
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return MapToDTO(customer);
        }

        public async Task<CustomerDTO> UpdateCustomerAsync(UpdateCustomerDTO updateDto)
        {
            var customer = await System.Threading.Tasks.Task.Run(() =>
                _context.Customers.FirstOrDefault(c => c.Id == updateDto.Id)
            );

            if (customer == null)
                return null;

            customer.Name = updateDto.Name;
            customer.PhoneNumber = updateDto.PhoneNumber;
            customer.Email = updateDto.Email;
            customer.Address = updateDto.Address;
            customer.City = updateDto.City;
            customer.District = updateDto.District;
            customer.Ward = updateDto.Ward;
            customer.DateOfBirth = updateDto.DateOfBirth;
            customer.Gender = updateDto.Gender;
            customer.LastModifiedDate = System.DateTime.Now;

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return MapToDTO(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await System.Threading.Tasks.Task.Run(() =>
                _context.Customers.FirstOrDefault(c => c.Id == id)
            );

            if (customer == null)
                return false;

            customer.IsActive = false;
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        private CustomerDTO MapToDTO(Customer customer)
        {
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
                TotalSpending = customer.TotalSpending,
                IsActive = customer.IsActive
            };
        }
    }
}
