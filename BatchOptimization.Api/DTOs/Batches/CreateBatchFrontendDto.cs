namespace BatchOptimization.Api.DTOs.Batches
{
    public class CreateBatchFrontendDto
    {
        public int SkuId { get; set; }   // Instead of SkuVersionId, frontend only passes SkuId
        public string BatchCode { get; set; } = null!;
        public double? BatchSize { get; set; }
    }
}
