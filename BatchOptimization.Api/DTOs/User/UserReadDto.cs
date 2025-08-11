namespace BatchOptimization.Api.DTOs.User
{
    public class UserReadDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int UserRoleId { get; set; }
        public bool IsActive { get; set; }
    }

}
