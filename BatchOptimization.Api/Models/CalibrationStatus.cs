using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class CalibrationStatus
{
    public int StatusId { get; set; }

    public string SensorId { get; set; } = null!;

    public bool AmFlag { get; set; }

    public bool WrFlag { get; set; }

    public string? AutoModulationStatus { get; set; }

    public string? WhiteReferenceStatus { get; set; }
}
