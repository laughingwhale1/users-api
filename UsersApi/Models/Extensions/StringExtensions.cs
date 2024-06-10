using System.Text.RegularExpressions;

namespace UsersApi.Models.Extensions;


public static class StringExtensions
{
    /// <summary>
    /// Converts to snake case.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>string</returns>
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var startUnderscores = Regex.Match(input, @"^_+");
        return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}