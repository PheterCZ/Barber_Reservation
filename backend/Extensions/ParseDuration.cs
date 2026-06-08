using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace backend.Extensions
{
    public static class ParseDuration
    {
        public static int ParseDurationMinutes(this string? serviceLabel)
        {
            var match = Regex.Match(serviceLabel ?? string.Empty, @"\((\d+)\s*min\)", RegexOptions.IgnoreCase);
            if (match.Success && int.TryParse(match.Groups[1].Value, out var parsedMinutes))
            {
                return Math.Clamp(parsedMinutes, 15, 240);
            }

            return 60;
        }
    }
}