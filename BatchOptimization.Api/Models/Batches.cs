using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class Batches
{
    public int BatchId { get; set; }

    public int SkuVersionId { get; set; }

    public string BatchCode { get; set; } = null!;

    public double? BatchSize { get; set; }

    public int BatchStatusId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual BatchStatuses BatchStatus { get; set; } = null!;

    public virtual Users CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Shots> Shots { get; set; } = new List<Shots>();

    public virtual SkuVersions SkuVersion { get; set; } = null!;

    public virtual Users? UpdatedByNavigation { get; set; }
}
