using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("DeliveryMethodCollection")]
    public class DeliveryMethodCollection
    {
        [DynamoDBHashKey("DeliveryMethodId")]
        public required string DeliveryMethodId { get; set; } = Guid.NewGuid().ToString();  
        [DynamoDBProperty("DeliveryMethod")]
        public required string DeliveryMethod { get; set; }
        [DynamoDBProperty("Cost")]
        public required decimal Cost { get; set; }
    }
}
