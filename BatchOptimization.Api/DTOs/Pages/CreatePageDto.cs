namespace BatchOptimization.Api.DTOs.Pages
{
    public class CreatePageDto
    {
        public string PageName { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
