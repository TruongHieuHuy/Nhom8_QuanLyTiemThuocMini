namespace PharmacyManagement.DTOs
{
    public class VnPayCreatePaymentResponseDTO
    {
        public string PaymentUrl { get; set; }
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public decimal Amount { get; set; }
    }
}
