using System;

namespace PharmacyManagement.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string RecipientType { get; set; } // Employee, Customer, All
        public int? EmployeeId { get; set; }
        public int? CustomerId { get; set; }
        public string NotificationType { get; set; } // Stock, Promotion, System
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
