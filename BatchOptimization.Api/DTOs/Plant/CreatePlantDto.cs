namespace BatchOptimization.Api.DTOs.Plant
{
    public class CreatePlantDto
    {
        public string PlantName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
