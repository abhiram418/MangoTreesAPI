using Amazon.DynamoDBv2.DataModel;
using MangoTreesAPI.Models;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("TransactionCollection")]
    public class TransactionCollection
    {
        [DynamoDBHashKey("OrderId")]
        public required string OrderId { get; set; }
        [DynamoDBProperty("TransactionId")]
        public required string TransactionId { get; set; } = Guid.NewGuid().ToString();
        [DynamoDBProperty("UserId")]
        public required string UserId { get; set; }
        [DynamoDBProperty("TransactionDate")]
        public required DateTime TransactionDate { get; set; }
        [DynamoDBProperty("PaymentMethod")]
        public required string PaymentMethod { get; set; }
        [DynamoDBProperty("Status")]
        public required OrderStatusEnum Status { get; set; }
    }
}
