using System;
using System.Collections.Generic;

namespace PharmacyManagement.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
    }

    public class CreateOrderDTO
    {
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
    }

    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CreateOrderDetailDTO
    {
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
    }
}
