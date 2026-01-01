using PharmacyManagement.DTOs;
using PharmacyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagement.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryHistoryDTO>> GetAllHistoriesAsync();
        Task<IEnumerable<InventoryHistoryDTO>> GetHistoriesByMedicineAsync(int medicineId);
        Task<IEnumerable<InventoryHistoryDTO>> GetHistoriesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<InventoryHistoryDTO> CreateInventoryHistoryAsync(CreateInventoryHistoryDTO createDto);
    }

    public class InventoryService : IInventoryService
    {
        private readonly PharmacyManagement.Data.PharmacyContext _context;

        public InventoryService(PharmacyManagement.Data.PharmacyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryHistoryDTO>> GetAllHistoriesAsync()
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return _context.InventoryHistories
                    .OrderByDescending(ih => ih.CreatedDate)
                    .Select(ih => MapToDTO(ih))
                    .ToList();
            });
        }

        public async Task<IEnumerable<InventoryHistoryDTO>> GetHistoriesByMedicineAsync(int medicineId)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return _context.InventoryHistories
                    .Where(ih => ih.MedicineId == medicineId)
                    .OrderByDescending(ih => ih.CreatedDate)
                    .Select(ih => MapToDTO(ih))
                    .ToList();
            });
        }

        public async Task<IEnumerable<InventoryHistoryDTO>> GetHistoriesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                return _context.InventoryHistories
                    .Where(ih => ih.CreatedDate >= startDate && ih.CreatedDate <= endDate)
                    .OrderByDescending(ih => ih.CreatedDate)
                    .Select(ih => MapToDTO(ih))
                    .ToList();
            });
        }

        public async Task<InventoryHistoryDTO> CreateInventoryHistoryAsync(CreateInventoryHistoryDTO createDto)
        {
            var medicine = await System.Threading.Tasks.Task.Run(() =>
                _context.Medicines.FirstOrDefault(m => m.Id == createDto.MedicineId)
            );

            if (medicine == null)
                return null;

            var history = new InventoryHistory
            {
                MedicineId = createDto.MedicineId,
                TransactionType = createDto.TransactionType,
                Quantity = createDto.Quantity,
                StockBefore = medicine.CurrentStock,
                StockAfter = createDto.TransactionType == "Import" 
                    ? medicine.CurrentStock + createDto.Quantity 
                    : medicine.CurrentStock - createDto.Quantity,
                Reason = createDto.Reason,
                Notes = createDto.Notes,
                CreatedDate = System.DateTime.Now
            };

            // Update medicine stock
            if (createDto.TransactionType == "Import")
                medicine.CurrentStock += createDto.Quantity;
            else if (createDto.TransactionType == "Export")
                medicine.CurrentStock -= createDto.Quantity;

            _context.InventoryHistories.Add(history);
            _context.Medicines.Update(medicine);
            await _context.SaveChangesAsync();

            return MapToDTO(history);
        }

        private InventoryHistoryDTO MapToDTO(InventoryHistory history)
        {
            return new InventoryHistoryDTO
            {
                Id = history.Id,
                MedicineId = history.MedicineId,
                MedicineName = history.Medicine?.Name,
                TransactionType = history.TransactionType,
                Quantity = history.Quantity,
                StockBefore = history.StockBefore,
                StockAfter = history.StockAfter,
                Reason = history.Reason,
                Notes = history.Notes,
                CreatedDate = history.CreatedDate
            };
        }
    }
}
