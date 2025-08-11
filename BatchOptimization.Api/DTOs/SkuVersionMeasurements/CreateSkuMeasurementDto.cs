namespace BatchOptimization.Api.DTOs.SkuVersionMeasurements
{
    public class CreateSkuMeasurementDto
    {

        public int SkuVersionId { get; set; }

        public string MeasurementType { get; set; } = null!;

        public double? MeasurementValue { get; set; }

    }
}
