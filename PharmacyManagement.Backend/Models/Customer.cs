using System;
using System.Collections.Generic;

namespace PharmacyManagement.Models
{
    public class Customer
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
        public decimal TotalSpending { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
