namespace MangoTreesAPI.Models 
{
    public class ResponseMessages
    {
        public static class Response
        {
            public static string Success = "Success";
            public static string Failure = "Failure";
            public static string ResourceNotFound = "The requested resource was not found.";
            public static string TryAgainLater = "Sorry for inconvenience please try again later";
        }
        public static class SignupResponse
        {
            public static string SuccessProperty1 = "User Registered";
            public static string SuccessProperty2 = "OTP Registered";
            public static string SuccessProperty3 = "Username Available";
            public static string FailureProperty1 = "The person trying to register or create an account on the MangoTrees already exists as a customer in the system";
            public static string FailureProperty2 = "OTP Expired or Not Valid";
        }
        public static class LoginResponse
        {
            public static string FailureProperty1 = "Check your Username or Password";
            public static string FailureProperty2 = "User credential didn't match";
            public static string FailureProperty3 = "User credential is not Active";
        }
        public static class PromotionCodeResponse
        {
            public static string SuccessProperty1 = "Promotion code is valid.";
            public static string SuccessProperty2 = "Promotion code Posted";
            public static string FailureProperty1 = "Invalid promotion code.";
            public static string FailureProperty2 = "This promotion code is not active or has expired";
        }
        public static class UpdateCustomerDetails
        {
            public static string SuccessProperty1 = "Customer Details Updated";
            public static string SuccessProperty2 = "Customer Password Updated";
        }
    }
}
