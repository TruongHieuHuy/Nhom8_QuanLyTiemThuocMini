using System;

namespace PharmacyManagement.DTOs
{
    public class PromotionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PromotionType { get; set; }
        public decimal DiscountValue { get; set; }
        public bool IsPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MinOrderAmount { get; set; }
        public string TargetCustomer { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreatePromotionDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PromotionType { get; set; }
        public decimal DiscountValue { get; set; }
        public bool IsPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MinOrderAmount { get; set; }
        public string TargetCustomer { get; set; }
    }

    public class UpdatePromotionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PromotionType { get; set; }
        public decimal DiscountValue { get; set; }
        public bool IsPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MinOrderAmount { get; set; }
        public string TargetCustomer { get; set; }
        public bool IsActive { get; set; }
    }
}
