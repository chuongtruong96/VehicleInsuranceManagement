using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class ContactU
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Topic { get; set; }

    public string? Phone { get; set; }

    public string? Message { get; set; }

    public bool? IsReplied { get; set; }

    public string? Title { get; set; }

    public string? Body { get; set; }
}
