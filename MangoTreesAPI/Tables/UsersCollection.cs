using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("UsersCollection")]
    public class UsersCollection
    {
        [DynamoDBHashKey("UserId")]
        public required string UserId { get; set; } = Guid.NewGuid().ToString();
        [DynamoDBProperty("FirstName")]
        public required string FirstName { get; set; }
        [DynamoDBProperty("LastName")]
        public required string LastName { get; set; }
        [DynamoDBProperty("Gender")]
        public required string Gender { get; set; }
        [DynamoDBProperty("DateOfBirth")]
        public required DateTime DateOfBirth { get; set; }
        [DynamoDBProperty("PhoneNumber")]
        public required string PhoneNumber { get; set; }
        [DynamoDBProperty("Email")]
        public string? Email { get; set; }
        [DynamoDBProperty("Role")]
        public required string Role { get; set; }
        [DynamoDBProperty("UserName")]
        public required string UserName { get; set; }
        [DynamoDBProperty("Password")]
        public required string Password { get; set; }
        [DynamoDBProperty("Occupation")]
        public required string Occupation { get; set; }
        [DynamoDBProperty("JoinDate")]
        public required DateTime JoinDate { get; set; }
        [DynamoDBProperty("Conditions")]
        public required bool Conditions { get; set; }
        [DynamoDBProperty("AddressList")]
        public required string[] AddressList { get; set; }
        [DynamoDBProperty("OrderHistory")]
        public required string[] OrderHistory { get; set; } = [];
        [DynamoDBProperty("Cart")]
        public required string Cart { get; set; } = string.Empty;
    }
}
