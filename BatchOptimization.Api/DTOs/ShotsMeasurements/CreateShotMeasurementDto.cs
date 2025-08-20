namespace BatchOptimization.Api.DTOs.ShotsMeasurements
{
    public class CreateShotMeasurementDto
    {
        public int ShotId { get; set; }

        public string MeasurementType { get; set; } = null!;

        public double? MeasurementValue { get; set; }
    }
}
