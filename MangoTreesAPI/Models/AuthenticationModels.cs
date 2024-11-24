namespace MangoTreesAPI.Models
{
    public class AuthenticationOptions
    {
        public required string SecretKey { get; set; }
    }

    public class LoginRequestModel
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }

    public class RolesModel
    {
        public required string RoleName { get; set; }
    }

    public class SessionModel
    {
        public required string SessionId { get; set; }
        public required string UserId { get; set; }
        public required string Token { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required DateTime ExpiresDate { get; set; }
    }

    public class UserAuthenticationModel
    {
        public required string UserName { get; set; }
        public required string UserId { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Role { get; set; }
        public required DateTime JoinDate { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
    }

}