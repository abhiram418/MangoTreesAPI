using Amazon.DynamoDBv2.DataModel;
using MangoTreesAPI.Tables;
using static System.Net.WebRequestMethods;

namespace MangoTreesAPI.Services
{
    public class OtpService
    {
        private readonly IDynamoDBContext context;
        public OtpService(IDynamoDBContext _context) {
            context = _context;
        }

        public async Task<string> GenerateSignupOTP(string phoneNumber)
        {
            Random random = new Random();
            int generatedOtp = random.Next(1000, 10000);
            string message = $"Welcome to MangoTrees! Your OTP for signing up is: {generatedOtp.ToString()}. Please use this to verify your phone number. This OTP is valid for 10 minutes.";
            
            await SaveOTP(phoneNumber, generatedOtp.ToString());
            SendOTP(phoneNumber, generatedOtp.ToString());

            return generatedOtp.ToString();
        }
        public async Task<string> GenerateUpdateCustomerPasswordOTP(string phoneNumber)
        {
            Random random = new Random();
            int generatedOtp = random.Next(1000, 10000);
            string message = $"Hello Mangoes! Your OTP for resetting your password is: {generatedOtp}. Please use this to verify your phone number. This OTP is valid for 10 minutes.";

            await SaveOTP(phoneNumber, generatedOtp.ToString());
            SendOTP(phoneNumber, generatedOtp.ToString());

            return generatedOtp.ToString();
        }
        private async Task SaveOTP(string phoneNumber, string otp)
        {
            var otpData = new OTPCollection() 
            { 
                PhoneNumber = phoneNumber,
                OTP = otp,
                CreatedDate = DateTime.UtcNow,
                ExpiresDate = DateTime.UtcNow.AddMinutes(10),
            };
            await context.SaveAsync(otpData);
        }
        private void SendOTP(string phoneNumber, string message)
        {
            Console.WriteLine(message);
        }
    }
}
