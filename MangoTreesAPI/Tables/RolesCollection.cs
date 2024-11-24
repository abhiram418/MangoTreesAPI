using Amazon.DynamoDBv2.DataModel;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("RolesCollection")]
    public class RolesCollection
    {
        [DynamoDBHashKey("RoleId")]
        public string RoleId { get; set; } = Guid.NewGuid().ToString();
        [DynamoDBProperty("RoleName")]
        public required string RoleName { get; set; } 
    }
}
