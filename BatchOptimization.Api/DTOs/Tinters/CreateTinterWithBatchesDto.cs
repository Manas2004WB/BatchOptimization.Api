using BatchOptimization.Api.DTOs.TinterBatches;
using BatchOptimization.Api.DTOs.TinterBatchMeasurement;

namespace BatchOptimization.Api.DTOs.Tinters
{
    public class CreateTinterWithBatchesDto
    {
        public CreateTinterDto Tinter { get; set; }
        public List<CreateTinterBatchesWithMeasurementsDto> Batches { get; set; }
    }

    public class CreateTinterBatchesWithMeasurementsDto
    {
        public CreateTinterBatchesDto Batch { get; set; }
        public List<CreateTinterBatchMeasurementDto> Measurements { get; set; }
    }
}
