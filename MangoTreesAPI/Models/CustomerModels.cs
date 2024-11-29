namespace MangoTreesAPI.Models
{
    public class CartModel
    {
        public required string UserId { get; set; }
        public string[] Items { get; set; } = [];
    }
    public class DeliveryMethodModel
    {
        public required string DeliveryMethod { get; set; }
        public required decimal Cost { get; set; }
    }
    public class OrderItemModel
    {
        public required string ProductId { get; set; }
        public required string ProductTitle { get; set; }
        public required string ProductDesc { get; set; }
        public required int Quantity { get; set; }
        public required decimal Price { get; set; }
        public required decimal TotalPrice { get; set; }
    }
    public class OrderRequestModel
    {
        public required DateTime OrderDate { get; set; }
        public required string ShippingAddress { get; set; }
        public required OrderItemModel[] OrderItems { get; set; }
        public required decimal TotalAmount { get; set; }
        public required string PaymentMethod { get; set; }
        public required DeliveryMethodModel DeliveryMethod { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Notes { get; set; }
        public bool? IsGift { get; set; }
        public string? GiftMessage { get; set; }
        public decimal? DiscountedAmount { get; set; }
        public string? PromotionApplied { get; set; }
    }

    public class OrderResponseModel
    {
        public required DateTime OrderDate { get; set; }
        public required string ShippingAddress { get; set; }
        public required OrderItemModel[] OrderItems { get; set; } = [];
        public required decimal TotalAmount { get; set; }
        public required string PaymentMethod { get; set; }
        public required OrderStatusEnum OrderStatus { get; set; } = OrderStatusEnum.AwaitingPayment;
        public required string DeliveryMethod { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Notes { get; set; }
        public bool? IsGift { get; set; }
        public string? GiftMessage { get; set; }
        public decimal? DiscountedAmount { get; set; }
        public string? PromotionApplied { get; set; }
    }

    public class OrderModel
    {
        public required DateTime OrderDate { get; set; }
        public required string ShippingAddress { get; set; }
        public required string[] OrderItems { get; set; }
        public required decimal TotalAmount { get; set; }
        public required string PaymentMethod { get; set; }
        public required OrderStatusEnum OrderStatus { get; set; } = OrderStatusEnum.AwaitingPayment;
        public required string DeliveryMethod { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Notes { get; set; }
        public bool? IsGift { get; set; }
        public string? GiftMessage { get; set; }
        public decimal? DiscountedAmount { get; set; }
        public string? PromotionApplied { get; set; }
    }
    public class AddressResponseModel
    {
        public string? AddressId { get; set; }
        public required string AddressTitle { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required int Pincode { get; set; }
        public required string State { get; set; }
        public required bool IsEditable { get; set; }
        public required bool IsDeleteable { get; set; }
        public required bool IsPrimary { get; set; }
    }
    public class AddressModel
    {
        public required string AddressTitle { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required int Pincode { get; set; }
        public required string State { get; set; }
        public required bool IsEditable { get; set; }
        public required bool IsDeleteable { get; set; }
        public required bool IsPrimary { get; set; }
    }
    public class ResetPasswordModel
    {
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required string OTP { get; set; }
    }
    public class UserEditDetailsModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Gender { get; set; }
        public string? Email { get; set; }
    }
    public class UserRequestModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Gender { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public required string Role { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Occupation { get; set; }
        public required DateTime JoinDate { get; set; }
        public required bool Conditions { get; set; }
        public required AddressModel Address { get; set; }
    }
    public class UsersModels
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Gender { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public required string Role { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Occupation { get; set; }
        public required DateTime JoinDate { get; set; }
        public required bool Conditions { get; set; }
        public required List<string> AddressList { get; set; }
        public required List<string> OrderHistory { get; set; } = [];
        public required string Cart { get; set; } = string.Empty;
    }
}
