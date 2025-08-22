using BatchOptimization.Api.DTOs.TinterBatchMeasurement;

namespace BatchOptimization.Api.DTOs.TinterBatches
{
    public class CreateTinterBatchWithMeasurementsDto
    {
        public int TinterId { get; set; }
        public string TinterBatchCode { get; set; } = null!;
        public string BatchTinterName { get; set; } = null!;
        public double? Strength { get; set; }
        public string? Comments { get; set; }
        public bool IsActive { get; set; }

        public List<CreateInlineTinterBatchMeasurementDto> Measurements { get; set; }
        = new List<CreateInlineTinterBatchMeasurementDto>();

    }
}
