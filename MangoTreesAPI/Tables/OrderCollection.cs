using Amazon.DynamoDBv2.DataModel;
using MangoTreesAPI.Models;

namespace MangoTreesAPI.Tables
{
    [DynamoDBTable("OrderCollection")]
    public class OrderCollection
    {
        [DynamoDBHashKey("OrderId")]
        public required string OrderId { get; set; } = Guid.NewGuid().ToString();
        [DynamoDBProperty("OrderDate")]
        public required DateTime OrderDate { get; set; }
        [DynamoDBProperty("Customer")]
        public required string Customer { get; set; }
        [DynamoDBProperty("ShippingAddress")]
        public required string ShippingAddress { get; set; }
        [DynamoDBProperty("OrderItems")]
        public required string[] OrderItems { get; set; }
        [DynamoDBProperty("TotalAmount")]
        public required decimal TotalAmount { get; set; }
        [DynamoDBProperty("PaymentMethod")]
        public required string PaymentMethod { get; set; }
        [DynamoDBProperty("OrderStatus")]
        public required OrderStatusEnum OrderStatus { get; set; }
        [DynamoDBProperty("DeliveryMethod")]
        public required string DeliveryMethod { get; set; }
        [DynamoDBProperty("TrackingNumber")]
        public string? TrackingNumber { get; set; }
        [DynamoDBProperty("Notes")]
        public string? Notes { get; set; }
        [DynamoDBProperty("IsGift")]
        public bool? IsGift { get; set; }
        [DynamoDBProperty("GiftMessage")]
        public string? GiftMessage { get; set; }
        [DynamoDBProperty("DiscountedAmount")]
        public decimal? DiscountedAmount { get; set; }
        [DynamoDBProperty("PromotionApplied")]
        public string? PromotionApplied { get; set; }
    }
}
