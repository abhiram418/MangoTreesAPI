namespace MangoTreesAPI.Models
{
    public class OTPModel
    {
        public required string PhoneNumber { get; set; }
        public string OTP { get; set; } = string.Empty;
        public required DateTime CreatedDate { get; set; }
        public required DateTime ExpiresDate { get; set; }
    }
}
