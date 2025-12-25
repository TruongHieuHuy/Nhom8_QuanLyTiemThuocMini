using System;
using System.Collections.Generic;

namespace PharmacyManagement.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; } // Cash, Card, Transfer
        public string OrderStatus { get; set; } // Pending, Completed, Cancelled
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
