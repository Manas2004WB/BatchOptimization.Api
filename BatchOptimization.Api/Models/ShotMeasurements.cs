using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class ShotMeasurements
{
    public int ShotMeasurementId { get; set; }

    public int ShotId { get; set; }

    public string MeasurementType { get; set; } = null!;

    public double? MeasurementValue { get; set; }

    public virtual Shots Shot { get; set; } = null!;
}
