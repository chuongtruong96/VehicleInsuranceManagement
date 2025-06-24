using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class AspNetUserRole
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public virtual NameRole Role { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
