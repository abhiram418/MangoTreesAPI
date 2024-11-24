using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("OTPCollection")]
    public class OTPCollection
    {
        [DynamoDBHashKey("PhoneNumber")]
        public required string PhoneNumber { get; set; }
        [DynamoDBProperty("OTPId")]
        public string OTPId { get; set; } = Guid.NewGuid().ToString();
        [DynamoDBProperty("OTP")]
        public string OTP { get; set; } = string.Empty;
        [DynamoDBProperty("CreatedDate")]
        public required DateTime CreatedDate { get; set; }
        [DynamoDBProperty("ExpiresDate")]
        public required DateTime ExpiresDate { get; set; }
    }
}
