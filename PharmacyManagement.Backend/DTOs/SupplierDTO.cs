using System;

namespace PharmacyManagement.DTOs
{
    public class SupplierDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string TaxNumber { get; set; }
        public decimal Debt { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateSupplierDTO
    {
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string TaxNumber { get; set; }
    }

    public class UpdateSupplierDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string TaxNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
