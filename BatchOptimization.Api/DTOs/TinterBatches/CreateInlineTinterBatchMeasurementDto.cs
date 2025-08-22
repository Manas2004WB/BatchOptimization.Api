namespace BatchOptimization.Api.DTOs.TinterBatchMeasurement
{
    public class CreateInlineTinterBatchMeasurementDto
    {
        public string MeasurementType { get; set; } = null!;
        public double? MeasurementValue { get; set; }
    }
}
