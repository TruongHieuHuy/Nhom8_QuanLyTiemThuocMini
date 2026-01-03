using PharmacyManagement.DTOs;
using PharmacyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagement.Services
{
    public interface IReportService
    {
        Task<decimal> GetRevenueByDateAsync(DateTime date);
        Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MedicineDTO>> GetTopSellingMedicinesAsync(int count = 10);
        Task<IEnumerable<CustomerDTO>> GetTopCustomersAsync(int count = 10);
        Task<IEnumerable<MedicineDTO>> GetOutOfStockMedicinesAsync();
        Task<int> GetTotalOrdersByDateAsync(DateTime date);
    }

    public class ReportService : IReportService
    {
        private readonly PharmacyManagement.Data.PharmacyContext _context;

        public ReportService(PharmacyManagement.Data.PharmacyContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetRevenueByDateAsync(DateTime date)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return _context.Orders
                    .Where(o => o.OrderDate.Date == date.Date && o.OrderStatus == "Completed")
                    .Sum(o => o.Total);
            });
        }

        public async Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return _context.Orders
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.OrderStatus == "Completed")
                    .Sum(o => o.Total);
            });
        }

        public async Task<IEnumerable<MedicineDTO>> GetTopSellingMedicinesAsync(int count = 10)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return _context.OrderDetails
                    .GroupBy(od => od.Medicine)
                    .OrderByDescending(g => g.Sum(od => od.Quantity))
                    .Take(count)
                    .Select(g => new MedicineDTO
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        Price = g.Key.Price,
                        CurrentStock = g.Key.CurrentStock
                    })
                    .ToList();
            });
        }

        public async Task<IEnumerable<CustomerDTO>> GetTopCustomersAsync(int count = 10)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return _context.Customers
                    .OrderByDescending(c => c.TotalSpending)
                    .Take(count)
                    .Select(c => new CustomerDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        PhoneNumber = c.PhoneNumber,
                        TotalSpending = c.TotalSpending
                    })
                    .ToList();
            });
        }

        public async Task<IEnumerable<MedicineDTO>> GetOutOfStockMedicinesAsync()
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return _context.Medicines
                    .Where(m => m.IsActive && m.CurrentStock == 0)
                    .Select(m => new MedicineDTO
                    {
                        Id = m.Id,
                        Name = m.Name,
                        CurrentStock = m.CurrentStock,
                        MinStockLevel = m.MinStockLevel
                    })
                    .ToList();
            });
        }

        public async Task<int> GetTotalOrdersByDateAsync(DateTime date)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return _context.Orders
                    .Where(o => o.OrderDate.Date == date.Date)
                    .Count();
            });
        }
    }
}
