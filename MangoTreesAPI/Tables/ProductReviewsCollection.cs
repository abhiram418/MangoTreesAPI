using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("ProductReviewsCollection")]
    public class ProductReviewsCollection
    {
        [DynamoDBHashKey("ProductReviewsId")]
        public string ProductReviewsId { get; set; } = Guid.NewGuid().ToString();
        [DynamoDBProperty("ReviewerName")]
        public required string ReviewerName { get; set; }
        [DynamoDBProperty("Rating")]
        public required int Rating { get; set; }
        [DynamoDBProperty("Title")]
        public required string Title { get; set; }
        [DynamoDBProperty("Date")]
        public required string Date { get; set; }
        [DynamoDBProperty("Review")]
        public required string Review { get; set; }

    }
}
