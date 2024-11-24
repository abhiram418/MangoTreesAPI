using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("PromotionCollection")]
    public class PromotionCollection
    {
        [DynamoDBHashKey("Code")]
        public required string Code { get; set; }
        [DynamoDBProperty("PromotionId")]
        public required string PromotionId { get; set; } = Guid.NewGuid().ToString();
        [DynamoDBProperty("DiscountPercentage")]
        public required int DiscountPercentage { get; set; }
        [DynamoDBProperty("ValidFrom")]
        public required DateTime ValidFrom { get; set; }
        [DynamoDBProperty("ValidUntil")]
        public required DateTime ValidUntil { get; set; }
        [DynamoDBProperty("IsActive")]
        public required bool IsActive { get; set; }
        [DynamoDBProperty("ApplicablePrice")]
        public required decimal ApplicablePrice { get; set; }
    }
}
