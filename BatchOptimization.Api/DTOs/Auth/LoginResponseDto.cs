﻿namespace BatchOptimization.Api.DTOs.Auth
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }

}
