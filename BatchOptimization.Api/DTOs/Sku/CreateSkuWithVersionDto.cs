public class CreateSkuWithVersionDto
{
    public int PlantId { get; set; }
    public string SkuCode { get; set; } = null!;

    public LabDto StdLiquid { get; set; } = null!;
    public LabDto PanelColor { get; set; } = null!;
    public LabDto SpectroColor { get; set; } = null!;

    public double TargetDeltaE { get; set; }
    public List<StdTinterDto> StdTinters { get; set; }
    public string? Comments { get; set; }
}
public class StdTinterDto
{
    public int TinterId { get; set; }
}

public class LabDto
{
    public double L { get; set; }
    public double A { get; set; }
    public double B { get; set; }
}
