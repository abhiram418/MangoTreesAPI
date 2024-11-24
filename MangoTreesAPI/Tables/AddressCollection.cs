using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("AddressCollection")]
    public class AddressCollection
    {
        [DynamoDBHashKey("AddressId")]
        public required string AddressId { get; set; } = Guid.NewGuid().ToString();
        [DynamoDBProperty("AddressTitle")]
        public required string AddressTitle { get; set; }
        [DynamoDBProperty("Address")]
        public required string Address { get; set; }
        [DynamoDBProperty("Pincode")]
        public required int Pincode { get; set; }
        [DynamoDBProperty("City")]
        public required string City { get; set; }
        [DynamoDBProperty("State")]
        public required string State { get; set; }
        [DynamoDBProperty("IsEditable")]
        public required bool IsEditable { get; set; }
        [DynamoDBProperty("IsDeleteable")]
        public required bool IsDeleteable { get; set; }
        [DynamoDBProperty("IsPrimary")]
        public required bool IsPrimary { get; set; }
    }
}
