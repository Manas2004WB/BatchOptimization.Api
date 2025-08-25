namespace BatchOptimization.Api.DTOs.SkuWithVersion
{
    public class SkuWithVersionsDto
    {
        public int SkuId { get; set; }
        public string SkuName { get; set; } = null!;

        public List<SkuVersionDto> SkuVersions { get; set; } = new();
    }

    public class SkuVersionDto
    {
        public int SkuVersionId { get; set; }
        public int SkuRevision { get; set; }
        public string SkuCode { get; set; } = null!;

        public MeasurementDto StdLiquid { get; set; } = new();
        public MeasurementDto PanelColor { get; set; } = new();
        public MeasurementDto SpectroColor { get; set; } = new();

        public List<TinterDto> StdTinters { get; set; } = new();

        public double? TargetDeltaE { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Comments { get; set; }
    }

    public class MeasurementDto
    {
        public double? L { get; set; }
        public double? A { get; set; }
        public double? B { get; set; }
    }

    public class TinterDto
    {
        public int TinterId { get; set; }
        public string TinterCode { get; set; } = null!;
    }

}
