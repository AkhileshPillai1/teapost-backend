using System.Text.RegularExpressions;

namespace TeaPost.Utilities
{
    public static class InputValidators
    {
        public static bool validateEmail(this string email)
        {
            try
            {
                // Regular expression for validating an Email
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

                // Return true if the email matches the pattern, false otherwise
                return regex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
