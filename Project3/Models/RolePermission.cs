using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class RolePermission
{
    public int Id { get; set; }

    public string? PermissionsId { get; set; }

    public string? RoleId { get; set; }
}
