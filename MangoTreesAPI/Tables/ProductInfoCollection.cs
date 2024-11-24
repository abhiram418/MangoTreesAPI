using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("ProductInfoCollection")]
    public class ProductInfoCollection
    {
        [DynamoDBHashKey("ProductInfoId")]
        public string ProductInfoId { get; set; } = Guid.NewGuid().ToString();
        [DynamoDBProperty("NumberOfMangoes")]
        public required int NumberOfMangoes { get; set; }
        [DynamoDBProperty("Weight")]
        public required int Weight { get; set; }
        [DynamoDBProperty("Variety")]
        public required string Variety { get; set; }
        [DynamoDBProperty("RipenessLevel")]
        public required string RipenessLevel { get; set; }
        [DynamoDBProperty("StorageInstructions")]
        public required string StorageInstructions { get; set; }
    }
}
