namespace BatchOptimization.Api.DTOs.SkuVersions
{
    public class CreateSkuVersionDto
    {
        public int SkuId { get; set; }

        public int VersionNumber { get; set; }

        public string VersionName { get; set; } = null!;

        public int ProductTypeId { get; set; }

        public int ColorimeterInstrumentId { get; set; }

        public bool IsDefault { get; set; }

        public string? Comments { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
