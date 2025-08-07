using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class CalibrationType
{
    public int CalibrationTypeId { get; set; }

    public string CalibrationTypeName { get; set; } = null!;

    public virtual ICollection<CalibrationData> CalibrationData { get; set; } = new List<CalibrationData>();
}
