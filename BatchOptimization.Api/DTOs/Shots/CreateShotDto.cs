namespace BatchOptimization.Api.DTOs.Shots
{
    public class CreateShotDto
    {
        public int BatchId { get; set; }

        public int ShotNumber { get; set; }

        public string? Comments { get; set; }
    }
}
