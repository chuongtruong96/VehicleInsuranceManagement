using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class CollectInfo
{
    public int Id { get; set; }

    public int DriverAge { get; set; }

    public string DriverGender { get; set; } = null!;

    public int DrivingHistory { get; set; }

    public string CustomerAdd { get; set; } = null!;

    public string Usage { get; set; } = null!;

    public bool AntiTheftDevice { get; set; }

    public bool MultiPolicy { get; set; }

    public bool SafeDriver { get; set; }

    public string? SelectedCoverages { get; set; }
}
