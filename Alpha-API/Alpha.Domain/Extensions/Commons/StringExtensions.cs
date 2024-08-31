using System.Text.RegularExpressions;

namespace Alpha.Domain.Extensions.Commons
{
    public static class StringExtensions
    {
        public static string Formater(this string format, params object[] args)
        {
            try
            {
                return string.IsNullOrEmpty(format) ? string.Empty : string.Format(format, args);
            }
            catch
            {
                return "";
            }
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
        }

    }
}
