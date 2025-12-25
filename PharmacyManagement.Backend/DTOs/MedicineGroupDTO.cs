namespace PharmacyManagement.DTOs
{
    public class MedicineGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateMedicineGroupDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateMedicineGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
