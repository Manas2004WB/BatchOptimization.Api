using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class WeightPredictions
{
    public int WeightPredictionId { get; set; }

    public int PredictionId { get; set; }

    public int TinterId { get; set; }

    public double PredictedWeight { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Users? CreatedByNavigation { get; set; }

    public virtual Predictions Prediction { get; set; } = null!;

    public virtual Tinters Tinter { get; set; } = null!;

    public virtual Users? UpdatedByNavigation { get; set; }
}
