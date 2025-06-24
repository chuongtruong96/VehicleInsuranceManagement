using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView
{
    public class CompanyBillingPolicyViewModel
    {
        public int Id { get; set; }
        
        public string? CustomerId { get; set; }
        
        public string? CustomerName { get; set; }
        
        public string? PolicyNumber { get; set; }
        
        public string? CustomerAddProve { get; set; }
        
        public string? CustomerPhoneNumber { get; set; }
        
        public string? BillNo { get; set; }
        
        public string? VehicleName { get; set; }
        
        public string? VehicleModel { get; set; }
        
        [Range(10, float.MaxValue, ErrorMessage = "Vehicle Rate must be at least 10.")]
        [Required]
        public float VehicleRate { get; set; }
        [Required]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "Vehicle Body Number must be exactly 17 characters long.")]
        [RegularExpression(@"^[A-Za-z0-9]{17}$", ErrorMessage = "Vehicle Body Number must be a 17 character alphanumeric string.")]
        public string? VehicleBodyNumber { get; set; }
        [StringLength(20, MinimumLength = 17, ErrorMessage = "Vehicle Engine Number must be between 10 and 20 characters long.")]

        [RegularExpression(@"^[A-Za-z0-9]{10,20}$", ErrorMessage = "Vehicle Engine Number must be between 10 and 20 characters long.")]
        [Required]
        public string? VehicleEngineNumber { get; set; }
        
        
        public DateTime? Date { get; set; }
        
        public float  Amount { get; set; }
        
        public string? PaymentStatus { get; set; }

        public string? PlaceOfAccident { get; set; }

        public string? DateOfAccident { get; set; }
    }
}
