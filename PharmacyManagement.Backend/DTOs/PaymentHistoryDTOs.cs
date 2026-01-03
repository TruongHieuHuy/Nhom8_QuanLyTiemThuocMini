using System;
using System.Collections.Generic;

namespace PharmacyManagement.DTOs
{
    public class PaymentHistoryItemDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderCode { get; set; }

        public string Provider { get; set; }
        public string PaymentMethod { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public string Status { get; set; }

        public string TxnRef { get; set; }
        public string TransactionNo { get; set; }
        public string ResponseCode { get; set; }
        public string BankCode { get; set; }
        public string PayDate { get; set; }

        public string RawData { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class PaymentHistoryResponseDTO
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<PaymentHistoryItemDTO> Items { get; set; } = new List<PaymentHistoryItemDTO>();
    }
}
