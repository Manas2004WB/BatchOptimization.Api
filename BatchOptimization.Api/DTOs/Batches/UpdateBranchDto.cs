namespace BatchOptimization.Api.DTOs.Batches
{
    public class UpdateBranchDto
    {
        public string BatchCode { get; set; } = null!;

        public double? BatchSize { get; set; }

        public int BatchStatusId { get; set; }
    }
}
