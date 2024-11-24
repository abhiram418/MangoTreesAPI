using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("ProductCollection")]
    public class ProductCollection
    {
        [DynamoDBHashKey("ProductId")]
        public string ProductId { get; set; } = Guid.NewGuid().ToString();
        [DynamoDBProperty("ProductTitle")]
        public required string ProductTitle { get; set; }
        [DynamoDBProperty("ProductDesc")]
        public required string ProductDesc { get; set; }
        [DynamoDBProperty("Price")]
        public required decimal Price { get; set; }
        [DynamoDBProperty("Availability")]
        public required bool Availability { get; set; }
        [DynamoDBProperty("AvailabilityTitle")]
        public required string AvailabilityTitle { get; set; }
        [DynamoDBProperty("Images")]
        public required string[] Images { get; set; }
        [DynamoDBProperty("Stars")]
        public required int Stars { get; set; }
        [DynamoDBProperty("NumberOfRatings")]
        public required int NumberOfRatings { get; set; }
        [DynamoDBProperty("InventoryId")]
        public required string InventoryId { get; set; }
        [DynamoDBProperty("NutritionFacts")]
        public string[]? NutritionFacts { get; set; } = [];
        [DynamoDBProperty("ProductInfo")]
        public required string ProductInfo { get; set; }
        [DynamoDBProperty("ProductReviews")]
        public required string[] ProductReviews { get; set; }
        [DynamoDBProperty("DealTitle")]
        public string? DealTitle { get; set; }
        [DynamoDBProperty("Discount")]
        public int? Discount { get; set; }
        [DynamoDBProperty("SalePrice")]
        public decimal? SalePrice { get; set; }
        [DynamoDBProperty("IsInUse")]
        public required bool IsInUse { get; set; }
    }
}
