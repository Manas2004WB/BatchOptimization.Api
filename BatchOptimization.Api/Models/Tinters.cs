using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class Tinters
{
    public int TinterId { get; set; }

    public int PlantId { get; set; }

    public string TinterCode { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Users CreatedByNavigation { get; set; } = null!;

    public virtual Plants Plant { get; set; } = null!;

    public virtual ICollection<StandardRecipes> StandardRecipes { get; set; } = new List<StandardRecipes>();

    public virtual ICollection<TinterBatches> TinterBatches { get; set; } = new List<TinterBatches>();

    public virtual Users? UpdatedByNavigation { get; set; }

    public virtual ICollection<WeightPredictions> WeightPredictions { get; set; } = new List<WeightPredictions>();
}
