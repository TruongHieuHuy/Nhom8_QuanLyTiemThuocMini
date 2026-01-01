using PharmacyManagement.DTOs;
using PharmacyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagement.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync();
        Task<SupplierDTO> GetSupplierByIdAsync(int id);
        Task<IEnumerable<SupplierDTO>> SearchSuppliersAsync(string searchTerm);
        Task<SupplierDTO> CreateSupplierAsync(CreateSupplierDTO createDto);
        Task<SupplierDTO> UpdateSupplierAsync(UpdateSupplierDTO updateDto);
        Task<bool> DeleteSupplierAsync(int id);
    }

    public class SupplierService : ISupplierService
    {
        private readonly PharmacyManagement.Data.PharmacyContext _context;

        public SupplierService(PharmacyManagement.Data.PharmacyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync()
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return _context.Suppliers
                    .Where(s => s.IsActive)
                    .Select(s => MapToDTO(s))
                    .ToList();
            });
        }

        public async Task<SupplierDTO> GetSupplierByIdAsync(int id)
        {
            var supplier = await System.Threading.Tasks.Task.Run(() =>
                _context.Suppliers.FirstOrDefault(s => s.Id == id)
            );
            return supplier != null ? MapToDTO(supplier) : null;
        }

        public async Task<IEnumerable<SupplierDTO>> SearchSuppliersAsync(string searchTerm)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return _context.Suppliers
                    .Where(s => s.IsActive && (
                        s.Name.Contains(searchTerm) ||
                        s.PhoneNumber.Contains(searchTerm) ||
                        s.Email.Contains(searchTerm)
                    ))
                    .Select(s => MapToDTO(s))
                    .ToList();
            });
        }

        public async Task<SupplierDTO> CreateSupplierAsync(CreateSupplierDTO createDto)
        {
            var supplier = new Supplier
            {
                Name = createDto.Name,
                ContactPerson = createDto.ContactPerson,
                PhoneNumber = createDto.PhoneNumber,
                Email = createDto.Email,
                Address = createDto.Address,
                City = createDto.City,
                TaxNumber = createDto.TaxNumber,
                Debt = 0,
                IsActive = true,
                CreatedDate = System.DateTime.Now,
                LastModifiedDate = System.DateTime.Now
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return MapToDTO(supplier);
        }

        public async Task<SupplierDTO> UpdateSupplierAsync(UpdateSupplierDTO updateDto)
        {
            var supplier = await System.Threading.Tasks.Task.Run(() =>
                _context.Suppliers.FirstOrDefault(s => s.Id == updateDto.Id)
            );

            if (supplier == null)
                return null;

            supplier.Name = updateDto.Name;
            supplier.ContactPerson = updateDto.ContactPerson;
            supplier.PhoneNumber = updateDto.PhoneNumber;
            supplier.Email = updateDto.Email;
            supplier.Address = updateDto.Address;
            supplier.City = updateDto.City;
            supplier.TaxNumber = updateDto.TaxNumber;
            supplier.IsActive = updateDto.IsActive;
            supplier.LastModifiedDate = System.DateTime.Now;

            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
            return MapToDTO(supplier);
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await System.Threading.Tasks.Task.Run(() =>
                _context.Suppliers.FirstOrDefault(s => s.Id == id)
            );

            if (supplier == null)
                return false;

            supplier.IsActive = false;
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
            return true;
        }

        private SupplierDTO MapToDTO(Supplier supplier)
        {
            return new SupplierDTO
            {
                Id = supplier.Id,
                Name = supplier.Name,
                ContactPerson = supplier.ContactPerson,
                PhoneNumber = supplier.PhoneNumber,
                Email = supplier.Email,
                Address = supplier.Address,
                City = supplier.City,
                TaxNumber = supplier.TaxNumber,
                Debt = supplier.Debt,
                IsActive = supplier.IsActive
            };
        }
    }
}
