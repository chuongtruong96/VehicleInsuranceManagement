using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class NameRole
{
    public string Id { get; set; } = null!;

    public string? NameRole1 { get; set; }

    public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; } = new List<AspNetUserRole>();
}
