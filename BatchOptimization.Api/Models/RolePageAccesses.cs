using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class RolePageAccesses
{
    public int RolePageAccessId { get; set; }

    public int UserRoleId { get; set; }

    public int PageId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Users? CreatedByNavigation { get; set; }

    public virtual Pages Page { get; set; } = null!;

    public virtual Users? UpdatedByNavigation { get; set; }

    public virtual UserRoles UserRole { get; set; } = null!;
}
