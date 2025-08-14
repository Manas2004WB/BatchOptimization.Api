namespace BatchOptimization.Api.DTOs.TinterBatchMeasurement
{
    public class UpdateTinterBatchMeasurementDto
    {
        public string MeasurementType { get; set; } = null!;
        public double? MeasurementValue { get; set; }
    }
}
