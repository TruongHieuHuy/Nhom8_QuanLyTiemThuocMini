using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagement.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }

        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public int? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        // SỬA: Đổi lại thành Employee để khớp với bảng Employees trong DB
        public virtual Employee Employee { get; set; } 

        public DateTime OrderDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; } 
        public string OrderStatus { get; set; } 
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}