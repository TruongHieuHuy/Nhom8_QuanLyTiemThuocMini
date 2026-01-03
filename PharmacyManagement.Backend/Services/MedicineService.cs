using PharmacyManagement.DTOs;
using PharmacyManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagement.Services
{
    public interface IMedicineService
    {
        Task<IEnumerable<MedicineDTO>> GetAllMedicinesAsync();
        Task<MedicineDTO> GetMedicineByIdAsync(int id);
        Task<IEnumerable<MedicineDTO>> SearchMedicinesAsync(string searchTerm);
        Task<IEnumerable<MedicineDTO>> GetMedicinesByGroupAsync(int groupId);
        Task<IEnumerable<MedicineDTO>> GetLowStockMedicinesAsync();
        Task<IEnumerable<MedicineDTO>> GetExpiredMedicinesAsync();
        Task<MedicineDTO> CreateMedicineAsync(CreateMedicineDTO createDto);
        Task<MedicineDTO> UpdateMedicineAsync(UpdateMedicineDTO updateDto);
        Task<bool> DeleteMedicineAsync(int id);
    }

    public class MedicineService : IMedicineService
    {
        private readonly PharmacyManagement.Data.PharmacyContext _context;

        public MedicineService(PharmacyManagement.Data.PharmacyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MedicineDTO>> GetAllMedicinesAsync()
        {
            var medicines = await System.Threading.Tasks.Task.Run(() =>
                _context.Medicines.Where(m => m.IsActive).ToList()
            );
            return medicines.Select(m => MapToDTO(m)).ToList();
        }

        public async Task<MedicineDTO> GetMedicineByIdAsync(int id)
        {
            var medicine = await System.Threading.Tasks.Task.Run(() =>
                _context.Medicines.FirstOrDefault(m => m.Id == id)
            );
            return medicine != null ? MapToDTO(medicine) : null;
        }

        public async Task<IEnumerable<MedicineDTO>> SearchMedicinesAsync(string searchTerm)
        {
            var medicines = await System.Threading.Tasks.Task.Run(() =>
                _context.Medicines.Where(m => m.IsActive && (
                    m.Name.Contains(searchTerm) ||
                    m.Manufacturer.Contains(searchTerm) ||
                    m.Barcode.Contains(searchTerm)
                )).ToList()
            );
            return medicines.Select(m => MapToDTO(m)).ToList();
        }

        public async Task<IEnumerable<MedicineDTO>> GetMedicinesByGroupAsync(int groupId)
        {
            var medicines = await System.Threading.Tasks.Task.Run(() =>
                _context.Medicines.Where(m => m.IsActive && m.MedicineGroupId == groupId).ToList()
            );
            return medicines.Select(m => MapToDTO(m)).ToList();
        }

        public async Task<IEnumerable<MedicineDTO>> GetLowStockMedicinesAsync()
        {
            var medicines = await System.Threading.Tasks.Task.Run(() =>
                _context.Medicines.Where(m => m.IsActive && m.CurrentStock <= m.MinStockLevel).ToList()
            );
            return medicines.Select(m => MapToDTO(m)).ToList();
        }

        public async Task<IEnumerable<MedicineDTO>> GetExpiredMedicinesAsync()
        {
            var medicines = await System.Threading.Tasks.Task.Run(() =>
                _context.Medicines.Where(m => m.IsActive && m.ExpiryDate <= System.DateTime.Now).ToList()
            );
            return medicines.Select(m => MapToDTO(m)).ToList();
        }

        public async Task<MedicineDTO> CreateMedicineAsync(CreateMedicineDTO createDto)
        {
            var medicine = new Medicine
            {
                Name = createDto.Name,
                Description = createDto.Description,
                MedicineGroupId = createDto.MedicineGroupId,
                Usage = createDto.Usage,
                Dosage = createDto.Dosage,
                Manufacturer = createDto.Manufacturer,
                Price = createDto.Price,
                ExpiryDate = createDto.ExpiryDate,
                CurrentStock = createDto.CurrentStock,
                MinStockLevel = createDto.MinStockLevel,
                Unit = createDto.Unit,
                Barcode = createDto.Barcode,
                CreatedDate = System.DateTime.Now,
                LastModifiedDate = System.DateTime.Now,
                IsActive = true
            };

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            // Tự động ghi vào InventoryHistories khi tạo thuốc mới
            if (createDto.CurrentStock > 0)
            {
                var history = new InventoryHistory
                {
                    MedicineId = medicine.Id,
                    TransactionType = "Import",
                    Quantity = createDto.CurrentStock,
                    StockBefore = 0,
                    StockAfter = createDto.CurrentStock,
                    Reason = "Nhập kho lần đầu",
                    Notes = $"Thêm mới thuốc với số lượng {createDto.CurrentStock}",
                    CreatedDate = System.DateTime.Now
                };
                _context.InventoryHistories.Add(history);
                await _context.SaveChangesAsync();
            }

            return MapToDTO(medicine);
        }

        public async Task<MedicineDTO> UpdateMedicineAsync(UpdateMedicineDTO updateDto)
        {
            var medicine = await System.Threading.Tasks.Task.Run(() =>
                _context.Medicines.FirstOrDefault(m => m.Id == updateDto.Id)
            );

            if (medicine == null)
                return null;

            // Lưu tồn kho cũ để so sánh
            var oldStock = medicine.CurrentStock;

            medicine.Name = updateDto.Name;
            medicine.Description = updateDto.Description;
            medicine.MedicineGroupId = updateDto.MedicineGroupId;
            medicine.Usage = updateDto.Usage;
            medicine.Dosage = updateDto.Dosage;
            medicine.Manufacturer = updateDto.Manufacturer;
            medicine.Price = updateDto.Price;
            medicine.ExpiryDate = updateDto.ExpiryDate;
            medicine.CurrentStock = updateDto.CurrentStock;
            medicine.MinStockLevel = updateDto.MinStockLevel;
            medicine.Unit = updateDto.Unit;
            medicine.Barcode = updateDto.Barcode;
            medicine.IsActive = updateDto.IsActive;
            medicine.LastModifiedDate = System.DateTime.Now;

            _context.Medicines.Update(medicine);
            await _context.SaveChangesAsync();

            // Ghi lịch sử thay đổi tồn kho nếu có sự thay đổi
            if (updateDto.CurrentStock != oldStock)
            {
                var transactionType = updateDto.CurrentStock > oldStock ? "Import" : "Export";
                var quantity = Math.Abs(updateDto.CurrentStock - oldStock);
                var reason = updateDto.CurrentStock > oldStock ? "Nhập thêm thuốc vào kho" : "Xuất điều chỉnh tồn kho";
                
                var history = new InventoryHistory
                {
                    MedicineId = medicine.Id,
                    TransactionType = transactionType,
                    Quantity = quantity,
                    StockBefore = oldStock,
                    StockAfter = updateDto.CurrentStock,
                    Reason = reason,
                    Notes = $"Cập nhật từ {oldStock} sang {updateDto.CurrentStock}",
                    CreatedDate = System.DateTime.Now
                };
                _context.InventoryHistories.Add(history);
                await _context.SaveChangesAsync();
            }

            return MapToDTO(medicine);
        }

        public async Task<bool> DeleteMedicineAsync(int id)
        {
            var medicine = await System.Threading.Tasks.Task.Run(() =>
                _context.Medicines.FirstOrDefault(m => m.Id == id)
            );

            if (medicine == null)
                return false;

            medicine.IsActive = false;
            _context.Medicines.Update(medicine);
            await _context.SaveChangesAsync();
            return true;
        }

        private MedicineDTO MapToDTO(Medicine medicine)
        {
            return new MedicineDTO
            {
                Id = medicine.Id,
                Name = medicine.Name,
                Description = medicine.Description,
                MedicineGroupId = medicine.MedicineGroupId,
                MedicineGroupName = medicine.MedicineGroup?.Name,
                Usage = medicine.Usage,
                Dosage = medicine.Dosage,
                Manufacturer = medicine.Manufacturer,
                Price = medicine.Price,
                ExpiryDate = medicine.ExpiryDate,
                CurrentStock = medicine.CurrentStock,
                MinStockLevel = medicine.MinStockLevel,
                Unit = medicine.Unit,
                Barcode = medicine.Barcode,
                IsActive = medicine.IsActive,
                IsLowStock = medicine.CurrentStock <= medicine.MinStockLevel,
                IsExpired = medicine.ExpiryDate <= System.DateTime.Now
            };
        }
    }
}
