using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView
{
    public class InsuranceProcessViewModel
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        public string? CustomerAdd { get; set; }
        [Required]
        [Phone]
        public string CustomerPhoneNumber { get; set; }
        [Required]
        public int VehicleId { get; set; }
        public string? PolicyNumber { get; set; }
        public string? PolicyDate { get; set; }
        public decimal? VehicleNumber { get; set; }

        public string? VehicleName { get; set; }
        [Required]
        public string VehicleModel { get; set; }
        public string? VehicleVersion { get; set; }
        [Required]
        public float VehicleRate { get; set; }
        public string? VehicleWarranty { get; set; }
        
        [Required]
        public int PolicyTypeId { get; set; }
        [Required]
        public int? WarrantyId { get; set; }

        [Required]
        public string VehicleBodyNumber { get; set; }

        [Required]
        public string VehicleEngineNumber { get; set; }
        public string? CustomerAddProve { get; set; }
        [Required]
        
        public decimal? PolicyDuration { get; set; }

        
    }
}
