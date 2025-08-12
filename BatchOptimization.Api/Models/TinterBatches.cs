using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class TinterBatches
{
    public int TinterBatchId { get; set; }

    public int TinterId { get; set; }

    public string TinterBatchCode { get; set; } = null!;

    public string BatchTinterName { get; set; } = null!;

    public double? Strength { get; set; }

    public string? Comments { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Users CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<ShotTinters> ShotTinters { get; set; } = new List<ShotTinters>();

    public virtual Tinters Tinter { get; set; } = null!;

    public virtual ICollection<TinterBatchMeasurements> TinterBatchMeasurements { get; set; } = new List<TinterBatchMeasurements>();

    public virtual Users UpdatedByNavigation { get; set; } = null!;
}
