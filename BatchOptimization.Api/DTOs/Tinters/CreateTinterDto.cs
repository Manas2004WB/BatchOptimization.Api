namespace BatchOptimization.Api.DTOs.Tinters
{
    public class CreateTinterDto
    {
        public int PlantId { get; set; }

        public string TinterCode { get; set; } = null!;

        public bool IsActive { get; set; }

    }
}
