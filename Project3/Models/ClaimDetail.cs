using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class ClaimDetail
{
    public int Id { get; set; }

    public decimal? ClaimNumber { get; set; }

    public decimal? PolicyNumber { get; set; }

    public DateOnly? PolicyStartDate { get; set; }

    public DateOnly? PolicyEndDate { get; set; }

    public string? CustomerName { get; set; }

    public string? PlaceOfAccident { get; set; }

    public string? DateOfAccident { get; set; }

    public decimal? InsuredAmount { get; set; }

    public decimal? ClaimableAmount { get; set; }
}
