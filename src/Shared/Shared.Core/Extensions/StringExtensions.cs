using System.Text.RegularExpressions;

namespace Shared.Core.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex _pattern = new("(?<=[a-z0-9])[A-Z]", RegexOptions.Compiled);

        public static string ToKebabCase(this string value) => _pattern?.Replace(value, m => "-" + m.Value)?.ToLower();
    }
}
