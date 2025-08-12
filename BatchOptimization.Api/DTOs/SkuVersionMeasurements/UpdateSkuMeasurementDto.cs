namespace BatchOptimization.Api.DTOs.SkuVersionMeasurements
{
    public class UpdateSkuMeasurementDto
    {
        public string MeasurementType { get; set; } = null!;

        public double? MeasurementValue { get; set; }
    }
}
