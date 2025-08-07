using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class CalibrationData
{
    public int CalibrationId { get; set; }

    public string SensorId { get; set; } = null!;

    public string CalibrationSourceUsed { get; set; } = null!;

    public DateTime CalibrationDatetime { get; set; }

    public double AbsoluteCalibrationL { get; set; }

    public double AbsoluteCalibrationA { get; set; }

    public double AbsoluteCalibrationB { get; set; }

    public double DailyCalibrationL { get; set; }

    public double DailyCalibrationA { get; set; }

    public double DailyCalibrationB { get; set; }

    public string DcSource { get; set; } = null!;

    public bool AutoModulation { get; set; }

    public bool Readjustment { get; set; }

    public double? DeltaE { get; set; }

    public int? CalibrationTypeId { get; set; }

    public bool? WhiteReference { get; set; }

    public string? AutoModulationStatus { get; set; }

    public string? WhiteReferenceStatus { get; set; }

    public string? Comments { get; set; }

    public double? DishCleanCheckL { get; set; }

    public double? DishCleanCheckA { get; set; }

    public double? DishCleanCheckB { get; set; }

    public string? DishCleanCheckSource { get; set; }

    public virtual CalibrationType? CalibrationType { get; set; }
}
