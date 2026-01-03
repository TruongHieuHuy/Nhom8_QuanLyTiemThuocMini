using System.Collections.Generic;

namespace PharmacyManagement.DTOs
{
    public class CreateOrderDTO
    {
        public int? CustomerId { get; set; }
        public int? EmployeeId { get; set; }

        // Cash / Banking / VNPay ...
        public string PaymentMethod { get; set; }

        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public string Notes { get; set; }

        public List<CreateOrderDetailDTO> OrderDetails { get; set; } = new List<CreateOrderDetailDTO>();
    }

    public class CreateOrderDetailDTO
    {
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderResultDTO
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public decimal Total { get; set; }
        public string OrderStatus { get; set; }
    }
}
