using System.Text.RegularExpressions;

namespace MangoTreesAPI.Services
{
    public class GeneralService
    {
        public bool IsPhoneNumber(string input)
        {
            string phoneNumberPattern = @"^\d{10}$";
            return Regex.IsMatch(input, phoneNumberPattern);
        }
    }
}
