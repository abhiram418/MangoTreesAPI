using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("InformationCollection")]
    public class InformationCollection
    {
        [DynamoDBHashKey("InformationId")]
        public required string InformationId { get; set; } = Guid.NewGuid().ToString();

        [DynamoDBProperty("InformationCreatedDate")]
        public required DateTime InformationCreatedDate { get; set; }

        [DynamoDBProperty("InformationTitle")]
        public required string InformationTitle { get; set; }

        [DynamoDBProperty("Details")]
        public InformationDetail[] Details { get; set; } = [];
    }

    public class InformationDetail
    {
        [DynamoDBProperty("Title")]
        public required string Title { get; set; } = string.Empty;

        [DynamoDBProperty("Description")]
        public required string Description { get; set; } = string.Empty;
    }
}
