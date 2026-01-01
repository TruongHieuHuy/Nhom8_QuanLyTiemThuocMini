using System;

namespace PharmacyManagement.DTOs
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; }
    }

    public class CreateEmployeeDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Salary { get; set; }
    }

    public class UpdateEmployeeDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; }
    }
}
