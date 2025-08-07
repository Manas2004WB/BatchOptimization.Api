using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class SkuVersionMeasurements
{
    public int SkuVersionMeasurementId { get; set; }

    public int SkuVersionId { get; set; }

    public string MeasurementType { get; set; } = null!;

    public double? MeasurementValue { get; set; }

    public virtual SkuVersions SkuVersion { get; set; } = null!;
}
