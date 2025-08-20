namespace BatchOptimization.Api.DTOs.Batches
{
    public class CreateBranchDto
    {
        public int SkuVersionId { get; set; }

        public string BatchCode { get; set; } = null!;

        public double? BatchSize { get; set; }

        public int BatchStatusId { get; set; }
    }
}
