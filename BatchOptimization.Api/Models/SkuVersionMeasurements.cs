using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BatchOptimization.Api.Models;

public partial class SkuVersionMeasurements
{
    public int SkuVersionMeasurementId { get; set; }

    public int SkuVersionId { get; set; }

    public string MeasurementType { get; set; } = null!;

    public double? MeasurementValue { get; set; }

    [JsonIgnore]

    public virtual SkuVersions SkuVersion { get; set; } = null!;
}
