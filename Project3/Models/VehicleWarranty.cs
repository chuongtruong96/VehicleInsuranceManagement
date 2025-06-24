using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class VehicleWarranty
{
    public int WarrantyId { get; set; }

    public string? WarrantyType { get; set; }

    public string? WarrantyDuration { get; set; }

    public string? WarrantyDetails { get; set; }

    public virtual ICollection<Estimate> Estimates { get; set; } = new List<Estimate>();

    public virtual ICollection<InsuranceProcess> InsuranceProcesses { get; set; } = new List<InsuranceProcess>();

    public virtual ICollection<InsuranceProduct> InsuranceProducts { get; set; } = new List<InsuranceProduct>();
}
