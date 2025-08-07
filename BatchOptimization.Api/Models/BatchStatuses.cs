using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class BatchStatuses
{
    public int BatchStatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<Batches> Batches { get; set; } = new List<Batches>();

    public virtual Users? CreatedByNavigation { get; set; }

    public virtual Users? UpdatedByNavigation { get; set; }
}
