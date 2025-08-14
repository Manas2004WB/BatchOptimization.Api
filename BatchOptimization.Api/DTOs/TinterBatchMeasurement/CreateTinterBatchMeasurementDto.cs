namespace BatchOptimization.Api.DTOs.TinterBatchMeasurement
{
    public class CreateTinterBatchMeasurementDto
    {
        public int TinterBatchId { get; set; }

        public string MeasurementType { get; set; } = null!;

        public double? MeasurementValue { get; set; }
    }
}
