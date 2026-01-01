using System;

namespace PharmacyManagement.DTOs
{
    public class InventoryHistoryDTO
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string TransactionType { get; set; }
        public int Quantity { get; set; }
        public int StockBefore { get; set; }
        public int StockAfter { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateInventoryHistoryDTO
    {
        public int MedicineId { get; set; }
        public string TransactionType { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
    }
}
