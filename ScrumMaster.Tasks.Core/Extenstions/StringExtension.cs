using System.Text.RegularExpressions;

namespace ScrumMaster.Tasks.Core.Extenstions
{
    public static class StringExtension
    {
        public static string DeleteDangerousChars(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var s = input;

            // usuń <script>...</script>
            s = Regex.Replace(s, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", "",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // usuń wszystkie tagi HTML
            s = Regex.Replace(s, @"<[^>]+>", "");

            // usuń javascript: i eventy typu onload=
            s = Regex.Replace(s, @"javascript\s*:", "", RegexOptions.IgnoreCase);
            s = Regex.Replace(s, @"on\w+\s*=", "", RegexOptions.IgnoreCase);

            // normalizacja białych znaków
            s = Regex.Replace(s, @"\s{2,}", " ").Trim();

            return s;
        }
    }
}
