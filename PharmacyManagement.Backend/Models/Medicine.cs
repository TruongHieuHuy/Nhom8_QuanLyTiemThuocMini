using System;
using System.Collections.Generic;

namespace PharmacyManagement.Models
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MedicineGroupId { get; set; }
        public MedicineGroup MedicineGroup { get; set; }
        public string Usage { get; set; }
        public string Dosage { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int CurrentStock { get; set; }
        public int MinStockLevel { get; set; }
        public string Unit { get; set; }
        public string Barcode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual ICollection<InventoryHistory> InventoryHistories { get; set; } = new List<InventoryHistory>();
    }
}
