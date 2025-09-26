namespace BatchOptimization.Api.DTOs.UserRoleDto
{
    public class CreateUserRoleDto
    {
        public string RoleName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
