using MangoTreesAPI.Tables;

namespace MangoTreesAPI.Models
{
    public class AwsDataOptions
    {
        public required string BucketName { get; set; }
        public required string Region { get; set; }
    }
    public class PromotionModel
    {
        public required string Code { get; set; }
        public required int DiscountPercentage { get; set; }
        public required DateTime ValidFrom { get; set; }
        public required DateTime ValidUntil { get; set; }
        public required bool IsActive { get; set; }
        public required decimal ApplicablePrice { get; set; }
    }
    public class TransactionModel
    {
        public string? TransactionId { get; set; }
        public required string UserId { get; set; }
        public required string OrderId { get; set; }
        public required DateTime TransactionDate { get; set; }
        public string? TrackingNumber { get; set; }
        public required string PaymentMethod { get; set; }
        public required OrderStatusEnum Status { get; set; }
        public required decimal Amount { get; set; }
    }

    public class InformationModel
    {
        public DateTime InformationCreatedDate { get; set; }
        public string InformationTitle { get; set; } = string.Empty;
        public InformationDetail[] Details { get; set; } = [];
    }

    public class InformationDetailModel
    {
        public required string Title { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
    }
}
