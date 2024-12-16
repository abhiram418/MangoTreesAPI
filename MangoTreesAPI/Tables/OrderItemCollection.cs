using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("OrderItemCollection")]
    public class OrderItemCollection
    {
        [DynamoDBHashKey("OrderItemId")]
        public required string OrderItemId { get; set; } = Guid.NewGuid().ToString();
        [DynamoDBProperty("ProductId")]
        public required string ProductId { get; set; }
        [DynamoDBProperty("ProductTitle")]
        public required string ProductTitle { get; set; }
        [DynamoDBProperty("ProductDesc")]
        public required string ProductDesc { get; set; }
        [DynamoDBProperty("Image")]
        public required string Image { get; set; }
        [DynamoDBProperty("Quantity")]
        public required int Quantity { get; set; }
        [DynamoDBProperty("Price")]
        public required decimal Price { get; set; }
        [DynamoDBProperty("TotalPrice")]
        public required decimal TotalPrice { get; set; }
        [DynamoDBProperty("IsReviewed")]
        public required bool IsReviewed { get; set; }
    }
}
