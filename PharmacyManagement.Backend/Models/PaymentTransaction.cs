using System;

namespace PharmacyManagement.Models
{
    public class PaymentTransaction
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public string OrderCode { get; set; }

        // e.g. VNPay, Cash, Banking
        public string Provider { get; set; }

        // same as Provider for now, but kept for future (ATM/QR/IB...)
        public string PaymentMethod { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "VND";

        // Pending / Success / Failed
        public string Status { get; set; }

        // VNPay fields
        public string TxnRef { get; set; } // usually orderCode
        public string TransactionNo { get; set; }
        public string ResponseCode { get; set; }
        public string BankCode { get; set; }
        public string PayDate { get; set; }

        // store raw return/ipn query as JSON for audit/debug
        public string RawData { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
