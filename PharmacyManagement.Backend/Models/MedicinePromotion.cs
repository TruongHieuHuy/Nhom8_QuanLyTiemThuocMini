namespace PharmacyManagement.Models
{
    public class MedicinePromotion
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }
    }
}
