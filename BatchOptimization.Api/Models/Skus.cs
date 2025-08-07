using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class Skus
{
    public int SkuId { get; set; }

    public int PlantId { get; set; }

    public string SkuName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Users CreatedByNavigation { get; set; } = null!;

    public virtual Plants Plant { get; set; } = null!;

    public virtual ICollection<SkuVersions> SkuVersions { get; set; } = new List<SkuVersions>();

    public virtual Users? UpdatedByNavigation { get; set; }
}
