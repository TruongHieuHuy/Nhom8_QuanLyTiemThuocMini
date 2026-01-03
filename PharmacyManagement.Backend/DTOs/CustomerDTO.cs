using System;
using System.Collections.Generic; // Nếu có lỗi ở đây thì giữ lại, không thì thôi

namespace PharmacyManagement.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public decimal TotalSpending { get; set; } // Giữ lại để hiển thị tổng chi tiêu (nếu muốn)
        
        // --- ĐÃ XÓA DÒNG OrderDTO Ở ĐÂY ---
    }

    public class CreateCustomerDTO
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
    }
}