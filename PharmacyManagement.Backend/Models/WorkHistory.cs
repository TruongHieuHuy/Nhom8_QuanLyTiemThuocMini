using System;

namespace PharmacyManagement.Models
{
    public class WorkHistory
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime WorkDate { get; set; }
        public string Shift { get; set; } // Morning, Afternoon, Evening
        public int OrdersProcessed { get; set; }
        public decimal Revenue { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
