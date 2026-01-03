using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagement.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; }

        // --- SỬA LẠI KHÓA NGOẠI LÀ SỐ NGUYÊN (INT) ---
        public int? UserAccountId { get; set; } 
        [ForeignKey("UserAccountId")]
        public virtual UserAccount UserAccount { get; set; }
        // ----------------------------------------------

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }

        
        public virtual ICollection<WorkHistory> WorkHistories { get; set; }
    }
}