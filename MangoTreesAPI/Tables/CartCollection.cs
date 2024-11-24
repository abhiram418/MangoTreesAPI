using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("CartCollection")]
    public class CartCollection
    {
        [DynamoDBHashKey("CartId")]
        public string CartId { get; set; } = Guid.NewGuid().ToString();
        [DynamoDBProperty("UserId")]
        public required string UserId { get; set; }
        [DynamoDBProperty("Items")]
        public string[] Items { get; set; } = [];
    }
}
