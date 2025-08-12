namespace BatchOptimization.Api.DTOs.TinterBatches
{
    public class UpdateTinterBatchDto
    {
        public int TinterId { get; set; }
        public string TinterBatchCode { get; set; } = null!;
        public string BatchTinterName { get; set; } = null!;
        public double Strength { get; set; }
        public string? Comments { get; set; }
        public bool IsActive { get; set; }
    }
}
