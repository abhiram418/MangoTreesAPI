using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("ChargesCollection")]
    public class ChargesCollection
    {
        [DynamoDBHashKey("ChargesId")]
        public string? ChargesId { get; set; }
        [DynamoDBProperty("Premium")]
        public required decimal Premium { get; set; }
        [DynamoDBProperty("PocketFriendly")]
        public required decimal PocketFriendly { get; set; }
        [DynamoDBProperty("Basic")]
        public required decimal Basic { get; set; }

        [DynamoDBProperty("Dedicated")]
        public required decimal Dedicated { get; set; }
        [DynamoDBProperty("ThirdParty")]
        public required decimal ThirdParty { get; set; }
        [DynamoDBProperty("APSRTC")]
        public required decimal APSRTC { get; set; }
    }
}
