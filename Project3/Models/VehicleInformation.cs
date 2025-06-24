using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class VehicleInformation
{
    public int Id { get; set; }

    public string? VehicleName { get; set; }

    public string? VehicleOwnerName { get; set; }

    public string? VehicleModel { get; set; }

    public string? VehicleVersion { get; set; }

    public double VehicleRate { get; set; }

    public string? VehicleBodyNumber { get; set; }

    public string? VehicleEngineNumber { get; set; }

    public string? VehicleNumber { get; set; }

    public virtual ICollection<Estimate> Estimates { get; set; } = new List<Estimate>();

    public virtual ICollection<InsuranceProcess> InsuranceProcesses { get; set; } = new List<InsuranceProcess>();
}
