using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView
{
    public class EstimateModelView
    {
        public string CustomerId { get; set; }
        public int EstimateNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string VehicleName { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleVersion { get; set; }
        public float VehicleRate { get; set; }

        [Required]
        public int PolicyTypeId { get; set; }
        public string PolicyTypeName { get; set; } = string.Empty;  // Initialize as empty string
        public string PolicyTypeDetails { get; set; } = string.Empty;  // Initialize as empty string

        [Required]
        public int WarrantyId { get; set; }
        public string WarrantyType { get; set; } = string.Empty;  // Initialize as empty string
        public string WarrantyDetails { get; set; } = string.Empty;  // Initialize as empty string

        public int VehicleId { get; set; }

        // This property seems to represent if policies exist or are available.
        // Renamed for clarity:
        // This property seems to represent if policies exist or are available.
        // Renamed for clarity:
        public bool HasActivePolicies { get; set; }
    }
}
