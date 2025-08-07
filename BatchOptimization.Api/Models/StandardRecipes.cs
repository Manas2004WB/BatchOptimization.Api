using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class StandardRecipes
{
    public int StandardRecipeId { get; set; }

    public int SkuVersionId { get; set; }

    public int TinterId { get; set; }

    public int UpdateNumber { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int UpdatedBy { get; set; }

    public virtual Users CreatedByNavigation { get; set; } = null!;

    public virtual SkuVersions SkuVersion { get; set; } = null!;

    public virtual Tinters Tinter { get; set; } = null!;

    public virtual Users UpdatedByNavigation { get; set; } = null!;
}
