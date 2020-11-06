using System.Text.RegularExpressions;

namespace Business.Extensions
{
    public static class EmailExtension
    {
        public static bool IsEmailValid(this string email)
        {
            return Regex.IsMatch(email, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
        }
    }
}
