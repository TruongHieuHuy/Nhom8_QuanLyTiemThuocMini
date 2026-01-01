using System;

namespace PharmacyManagement.Models
{
    public class InventoryHistory
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }
        public string TransactionType { get; set; } // Import, Export, Adjust
        public int Quantity { get; set; }
        public int StockBefore { get; set; }
        public int StockAfter { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
