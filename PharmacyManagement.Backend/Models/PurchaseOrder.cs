using System;
using System.Collections.Generic;

namespace PharmacyManagement.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string PurchaseOrderCode { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; } // Pending, Received, Cancelled
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();
    }
}
