using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class ShotTinters
{
    public int ShotTinterId { get; set; }

    public int ShotId { get; set; }

    public int TinterBatchId { get; set; }

    public double TinterWeight { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Users CreatedByNavigation { get; set; } = null!;

    public virtual Shots Shot { get; set; } = null!;

    public virtual TinterBatches TinterBatch { get; set; } = null!;

    public virtual Users? UpdatedByNavigation { get; set; }
}
