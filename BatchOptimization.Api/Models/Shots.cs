using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class Shots
{
    public int ShotId { get; set; }

    public int BatchId { get; set; }

    public int ShotNumber { get; set; }

    public string? Comments { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int UpdatedBy { get; set; }

    public virtual Batches Batch { get; set; } = null!;

    public virtual Users CreatedByNavigation { get; set; } = null!;

    public virtual Predictions? Predictions { get; set; }

    public virtual ICollection<ShotMeasurements> ShotMeasurements { get; set; } = new List<ShotMeasurements>();

    public virtual ICollection<ShotTinters> ShotTinters { get; set; } = new List<ShotTinters>();

    public virtual Users UpdatedByNavigation { get; set; } = null!;
}
