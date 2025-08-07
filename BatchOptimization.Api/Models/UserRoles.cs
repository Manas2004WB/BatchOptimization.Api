using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class UserRoles
{
    public int UserRoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<RolePageAccesses> RolePageAccesses { get; set; } = new List<RolePageAccesses>();

    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}
