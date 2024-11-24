using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("UserAuthenticationCollection")]
    public class UserAuthenticationCollection
    {
        [DynamoDBHashKey("UserName")]
        public required string UserName { get; set; }
        [DynamoDBProperty("UserId")]
        public required string UserId { get; set; }
        [DynamoDBProperty("Password")]
        public required string Password { get; set; }
        [DynamoDBProperty("PhoneNumber")]
        public required string PhoneNumber { get; set; }
        [DynamoDBProperty("Email")]
        public string? Email { get; set; }
        [DynamoDBProperty("Role")]
        public required string Role { get; set; }
        [DynamoDBProperty("JoinDate")]
        public required DateTime JoinDate { get; set; }
        [DynamoDBProperty("UpdatedDate")]
        public required DateTime UpdatedDate { get; set; }
        [DynamoDBProperty("IsActive")]
        public required bool IsActive { get; set; }
        [DynamoDBProperty("LastLogin")]
        public required DateTime LastLogin { get; set; }
    }
}
