using BatchOptimization.Api.DTOs.SkuWithVersion;

namespace BatchOptimization.Api.DTOs.Plant
{
    public class GetPlantDto
    {
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public bool IsActive { get; set; }
        public List<SkuDto> Skus { get; set; }
        public List<TinterDto> Tinters { get; set; }
    }
    public class SkuDto
    {
        public int SkuId { get; set; }
        public string SkuName { get; set; }
    }

    public class TinterDto
    {
        public int TinterId { get; set; }
        public string TinterName { get; set; }
    }
}
