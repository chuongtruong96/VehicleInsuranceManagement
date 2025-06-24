using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class CompanyBillingPolicy
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

    public double VehicleRate { get; set; }

    public string? VehicleBodyNumber { get; set; }

    public string? VehicleEngineNumber { get; set; }

    public DateTime? Date { get; set; }

    public double Amount { get; set; }

    public string? PaymentStatus { get; set; }

    public virtual AspNetUser? Customer { get; set; }
}
