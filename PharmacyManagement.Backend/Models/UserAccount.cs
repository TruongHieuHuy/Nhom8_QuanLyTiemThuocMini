using System;
using System.Collections.Generic;

namespace PharmacyManagement.Models
{
    public class UserAccount
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // Admin, Manager, Cashier, Pharmacist
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastLoginDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
