namespace MangoTreesAPI.Models
{
    public class InventoryRequestModel
    {
        public DateTime? ExpirationDate { get; set; }
        public required int StockQuantity { get; set; }
    }
    public class ChargesModel
    {
        public required decimal Premium { get; set; }
        public required decimal PocketFriendly { get; set; }
        public required decimal Basic { get; set; }
        public required decimal Dedicated { get; set; }
        public required decimal ThirdParty { get; set; }
        public required decimal APSRTC { get; set; }
    }
    public class InventoryModel
    {
        public string ProductId { get; set; } = string.Empty;
        public required DateTime StockAddedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpirationDate { get; set; }
        public required int StockQuantity { get; set; }
    }
    public class ProductReviewsModel
    {
        public required string ReviewerName { get; set; }
        public required string Rating { get; set; }
        public required string Title { get; set; }
        public required string Date { get; set; }
        public required string Review { get; set; }
    }
    public class ProductInfoResponceModel
    {
        public string ProductId { get; set; } = string.Empty;
        public required int NumberOfMangoes { get; set; }
        public required int Weight { get; set; }
        public required string Variety { get; set; }
        public required string RipenessLevel { get; set; }
        public required string StorageInstructions { get; set; }
    }
    public class ProductInfoModel
    {
        public required int NumberOfMangoes { get; set; }
        public required int Weight { get; set; }
        public required string Variety { get; set; }
        public required string RipenessLevel { get; set; }
        public required string StorageInstructions { get; set; }
    }
    public class ProductRequestModel
    {
        public required string ProductTitle { get; set; }
        public required string ProductDesc { get; set; }
        public required decimal Price { get; set; }
        public required bool Availability { get; set; }
        public required string AvailabilityTitle { get; set; }
        public required IFormFile[] Images { get; set; }
        public int Stars { get; set; } = 0;
        public int NumberOfRatings { get; set; } = 0;
        public string[]? NutritionFacts { get; set; }
        public required InventoryRequestModel Inventory { get; set; }
        public required ProductInfoModel ProductInfo { get; set; }
        public List<string> ProductReviews { get; set; } = [];
        public string? DealTitle { get; set; }
        public int? Discount { get; set; }
        public decimal? SalePrice { get; set; }
        public required bool IsInUse { get; set; }
    }
    public class ProductModel
    {
        public string? ProductId { get; set; }
        public required string ProductTitle { get; set; }
        public required string ProductDesc { get; set; }
        public required decimal Price { get; set; }
        public required bool Availability { get; set; }
        public required string AvailabilityTitle { get; set; }
        public required List<string> Images { get; set; }
        public required int Stars { get; set; }
        public required int NumberOfRatings { get; set; }
        public string[]? NutritionFacts { get; set; }
        public required string InventoryId { get; set; } = string.Empty;
        public required string ProductInfo { get; set; }
        public required List<string> ProductReviews { get; set; }
        public string? DealTitle { get; set; }
        public int? Discount { get; set; }
        public decimal? SalePrice { get; set; }
        public required bool IsInUse { get; set; }
    }
}
