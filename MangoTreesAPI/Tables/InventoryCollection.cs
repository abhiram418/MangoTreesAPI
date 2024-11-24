using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("InventoryCollection")]
    public class InventoryCollection
    {
        [DynamoDBHashKey("InventoryId")]
        public required string InventoryId = Guid.NewGuid().ToString();
        [DynamoDBProperty("ProductId")]
        public required string ProductId {  get; set; }
        [DynamoDBProperty("StockAddedDate")]
        public required DateTime StockAddedDate { get; set; }
        [DynamoDBProperty("ExpirationDate")]
        public DateTime? ExpirationDate { get; set; }
        [DynamoDBProperty("StockQuantity")]
        public required int StockQuantity { get; set; }
    }
}
