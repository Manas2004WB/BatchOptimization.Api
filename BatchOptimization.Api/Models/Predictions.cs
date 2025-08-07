using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class Predictions
{
    public int PredictionId { get; set; }

    public int ShotId { get; set; }

    public double DlMin { get; set; }

    public double DlMax { get; set; }

    public double DaMin { get; set; }

    public double DaMax { get; set; }

    public double DbMin { get; set; }

    public double DbMax { get; set; }

    public double DeMin { get; set; }

    public double DeMax { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Users? CreatedByNavigation { get; set; }

    public virtual Shots Shot { get; set; } = null!;

    public virtual Users? UpdatedByNavigation { get; set; }

    public virtual ICollection<WeightPredictions> WeightPredictions { get; set; } = new List<WeightPredictions>();
}
