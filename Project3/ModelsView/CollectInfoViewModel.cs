using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView
{
    public class CollectInfoViewModel
    {

        public string? VehicleName { get; set; }

        public string? VehicleModel { get; set; }
        public string? VehicleVersion { get; set; }

        public float VehicleRate { get; set; }

        public string? VehicleBodyNumber { get; set; }

        public string? VehicleEngineNumber { get; set; }

        [Required]
        [Range(18, 100)]
        public int DriverAge { get; set; }

        [Required]
        public string DriverGender { get; set; }

        [Required]
        [Range(0, 100)]
        public int DrivingHistory { get; set; } // Number of accidents or violations

        [Required]
        public string CustomerAdd { get; set; } // Residential area

        [Required]
        public string Usage { get; set; } // Usage of the vehicle

        public bool AntiTheftDevice { get; set; } // Whether an anti-theft device is installed

        public bool MultiPolicy { get; set; } // Whether the customer has multiple policies

        public bool SafeDriver { get; set; } // Whether the customer is a safe driver
        public string? SelectedCoverages { get; set; }


    }
}
