using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class Plants
{
    public int PlantId { get; set; }

    public string PlantName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Users CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Skus> Skus { get; set; } = new List<Skus>();

    public virtual ICollection<Tinters> Tinters { get; set; } = new List<Tinters>();

    public virtual Users? UpdatedByNavigation { get; set; }
}
