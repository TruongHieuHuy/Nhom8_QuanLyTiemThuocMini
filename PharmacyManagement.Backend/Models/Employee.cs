using System;
using System.Collections.Generic;

namespace PharmacyManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; } // Active, Inactive, On Leave
        public string UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<WorkHistory> WorkHistories { get; set; } = new List<WorkHistory>();
    }
}
