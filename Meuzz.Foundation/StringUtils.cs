using System.Linq;
using System.Text;

namespace Meuzz.Foundation
{
    public static class StringUtilsExtensions
    {
        public static string ToSnake(this string s, bool toupper = false)
        {
            var ret = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                var c = s[i];
                if (i > 0 && 'A' <= c && c <= 'Z' && s[i - 1] != '_')
                {
                    ret.Append('_');
                }

                ret.Append(toupper ? char.ToUpper(c) : char.ToLower(c));
            }
            return ret.ToString();
        }

        public static string ToCamel(this string s, bool upperCamel = false)
        {
            var ss = s.ToLower().Split('_');
            if (ss.Length == 1)
            {
                return !upperCamel ? s : Capitalize(s);
            }
            return upperCamel
                ? string.Join(string.Empty, ss.Select(x => Capitalize(x)))
                : string.Join(string.Empty, ss.Select((x, i) => i == 0 && !upperCamel ? x.ToLower() : Capitalize(x)));
        }

        public static string Capitalize(this string s)
        {
            if (s.Length == 0) { return string.Empty; }
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
