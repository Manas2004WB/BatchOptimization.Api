namespace BatchOptimization.Api.DTOs.ShotTinters
{
    public class CreateShotTinterDto
    {
        public int ShotId { get; set; }

        public int TinterBatchId { get; set; }

        public double TinterWeight { get; set; }
    }
}
