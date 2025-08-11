namespace BatchOptimization.Api.DTOs.User
{
        public class UserCreateDto
        {
            public string Username { get; set; }
            public string Password { get; set; } // Plain text password to be hashed
            public string Email { get; set; }
            public int UserRoleId { get; set; }
            public int CreatedBy { get; set; }   // Optional
        }
}
