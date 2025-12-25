using System;
using System.Collections.Generic;

namespace PharmacyManagement.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PromotionType { get; set; } // Discount, Gift
        public decimal DiscountValue { get; set; }
        public bool IsPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MinOrderAmount { get; set; }
        public string TargetCustomer { get; set; } // All, VIP, Regular
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual ICollection<MedicinePromotion> MedicinePromotions { get; set; } = new List<MedicinePromotion>();
    }
}
