using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView;

public partial class VehicleInformationViewModel
{
    public int Id { get; set; }

    [Required]
    public string? VehicleName { get; set; }

    [Required]
    public string? VehicleModel { get; set; }

    [Required]
    public string? VehicleVersion { get; set; }

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
    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Vehicle Number must be exactly 6 characters long.")]
    [RegularExpression(@"^[A-Z]{2}\d{4}$", ErrorMessage = "Vehicle Number must be in the format of two letters followed by four digits (e.g., AB1234).")]
    public string? VehicleNumber { get; set; }

}
