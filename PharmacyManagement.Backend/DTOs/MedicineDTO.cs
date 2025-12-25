using System;

namespace PharmacyManagement.DTOs
{
    public class MedicineDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MedicineGroupId { get; set; }
        public string MedicineGroupName { get; set; }
        public string Usage { get; set; }
        public string Dosage { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int CurrentStock { get; set; }
        public int MinStockLevel { get; set; }
        public string Unit { get; set; }
        public string Barcode { get; set; }
        public bool IsActive { get; set; }
        public bool IsLowStock { get; set; }
        public bool IsExpired { get; set; }
    }

    public class CreateMedicineDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MedicineGroupId { get; set; }
        public string Usage { get; set; }
        public string Dosage { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int CurrentStock { get; set; }
        public int MinStockLevel { get; set; }
        public string Unit { get; set; }
        public string Barcode { get; set; }
    }

    public class UpdateMedicineDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MedicineGroupId { get; set; }
        public string Usage { get; set; }
        public string Dosage { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int MinStockLevel { get; set; }
        public string Unit { get; set; }
        public string Barcode { get; set; }
        public bool IsActive { get; set; }
    }
}
