using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class SkuVersions
{
    public int SkuVersionId { get; set; }

    public int SkuId { get; set; }

    public int VersionNumber { get; set; }

    public string VersionName { get; set; } = null!;

    public int ProductTypeId { get; set; }

    public int ColorimeterInstrumentId { get; set; }

    public bool IsDefault { get; set; }

    public string? Comments { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int UpdatedBy { get; set; }

    public virtual ICollection<Batches> Batches { get; set; } = new List<Batches>();

    public virtual Users CreatedByNavigation { get; set; } = null!;

    public virtual Skus Sku { get; set; } = null!;

    public virtual ICollection<SkuVersionMeasurements> SkuVersionMeasurements { get; set; } = new List<SkuVersionMeasurements>();

    public virtual ICollection<StandardRecipes> StandardRecipes { get; set; } = new List<StandardRecipes>();

    public virtual Users UpdatedByNavigation { get; set; } = null!;
}
