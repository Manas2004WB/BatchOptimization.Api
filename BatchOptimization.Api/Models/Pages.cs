using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class Pages
{
    public int PageId { get; set; }

    public string PageName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Users? CreatedByNavigation { get; set; }

    public virtual ICollection<RolePageAccesses> RolePageAccesses { get; set; } = new List<RolePageAccesses>();

    public virtual Users? UpdatedByNavigation { get; set; }
}
