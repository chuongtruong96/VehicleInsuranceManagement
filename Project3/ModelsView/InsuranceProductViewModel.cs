namespace Project3.ModelsView
{
    public class InsuranceProductViewModel
    {
        public int PolicyTypeId { get; set; }  // ID of the policy type
        public string? PolicyName { get; set; }  // Name of the policy
        public string? PolicyDetails { get; set; }  // Detailed description of the policy

        public int WarrantyId { get; set; }  // ID of the warranty
        public string? WarrantyType { get; set; }  // Type of the warranty
        public string? WarrantyDuration { get; set; }  // Duration of the warranty (e.g., "3 years")
        public string? WarrantyDetails { get; set; }  // Detailed description of the warranty

        public float VehicleRate { get; set; }  // The rate or cost associated with the insurance product
        public string? ImageUrl { get; set; }  // URL of the image associated with the product
    }
}
