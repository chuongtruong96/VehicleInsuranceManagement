using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class InsuranceProduct
{
    public int InsuranceProductId { get; set; }

    public int PolicyTypeId { get; set; }

    public int WarrantyId { get; set; }

    public double VehicleRate { get; set; }

    public virtual VehiclePolicyType PolicyType { get; set; } = null!;

    public virtual VehicleWarranty Warranty { get; set; } = null!;
}
