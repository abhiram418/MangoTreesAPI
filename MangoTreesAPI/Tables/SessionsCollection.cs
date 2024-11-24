using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("SessionsCollection")]
    public class SessionsCollection
    {
        [DynamoDBHashKey("SessionId")]
        public required string SessionId { get; set; }
        [DynamoDBProperty("UserId")]
        public required string UserId { get; set; }
        [DynamoDBProperty("Token")]
        public required string Token { get; set; }
        [DynamoDBProperty("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [DynamoDBProperty("ExpiresDate")]
        public DateTime ExpiresDate { get; set; }
    }
}
