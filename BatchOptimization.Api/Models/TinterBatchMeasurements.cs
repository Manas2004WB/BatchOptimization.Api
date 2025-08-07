using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class TinterBatchMeasurements
{
    public int TinterBatchMeasurementId { get; set; }

    public int TinterBatchId { get; set; }

    public string MeasurementType { get; set; } = null!;

    public double? MeasurementValue { get; set; }

    public virtual TinterBatches TinterBatch { get; set; } = null!;
}
