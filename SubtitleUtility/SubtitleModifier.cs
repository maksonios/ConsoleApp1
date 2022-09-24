using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SubtitleUtility;

public static class SubtitleModifier
{
    private const string DefaultTimeIntervalDelimiter = "-->";
    private const string TimeFormat = @"hh\:mm\:ss\,fff";
    private static readonly Regex TimeRegex = new(@"(\d\d):(\d\d):(\d\d),(\d\d\d)");
    
    public static string ExecuteSubtitleShift(string input,
                                              int shiftMs, 
                                              string sourceTimeIntervalDelimiter = DefaultTimeIntervalDelimiter, 
                                              string targetTimeIntervalDelimiter = DefaultTimeIntervalDelimiter,
                                              bool isSubtitleNumberingEnabled = true)
    {
        var source = Regex.Split(input, "\r\n|\r|\n");

        ValidateTimeIntervalDelimiterConsistency(input);
        ReplaceTimeIntervalDelimiter(input, targetTimeIntervalDelimiter);
        
        var newStr = new StringBuilder();
        if (!isSubtitleNumberingEnabled)
        {
            string matchSymbol = "-->";
            for (int i = 0; i < source.Length-1; i++)
            {
                if (source[i+1].Contains(matchSymbol))
                {
                    var timeline=TimeRegex.Replace(source[i+1], m => AddTime(m, shiftMs));
                    newStr.AppendLine(timeline);
                    continue;
                }
                if (source[i].Contains(matchSymbol))
                {
                    continue;
                }
                newStr.AppendLine(source[i]);
            }
        }
        else
        {
            var result = TimeRegex.Replace(string.Join(Environment.NewLine, source), m => AddTime(m, shiftMs));
            return result;
        }

        return newStr.ToString();
    }

    private static void ValidateTimeIntervalDelimiterConsistency(string input)
    {
        var source = Regex.Split(input, "\r\n|\r|\n");
        for (int i = 0; i < source.Length-1; i++)
        {
            if (source[i].IndexOf(":") == 2)
            {
                string delimiter = source[i].Substring(13, 3);
                if (delimiter == DefaultTimeIntervalDelimiter)
                {
                    continue;
                }
                else
                {
                    throw new InvalidDataException($"TimeIntervalDelimiterConsistency is not consistent across file. The subtitle line: #{i + 1}");
                }
            }
        }
    }

    private static string ReplaceTimeIntervalDelimiter(string input, string targetTimeIntervalDelimiter)
    {
        var source = Regex.Split(input, "\r\n|\r|\n");
        for (int i = 0; i < source.Length - 1; i++)
        {
            if (source[i].IndexOf(":") == 2)
            {
                string delimeter = source[i].Substring(13, 3);
                string temp = source[i].Replace(delimeter, targetTimeIntervalDelimiter);
                source[i] = temp;
            }
        }

        return source.ToString();
    }

    private static string AddTime(Match m, int shiftMs)
    {
        var t = TimeSpan.ParseExact(m.Value, TimeFormat, CultureInfo.InvariantCulture);
        t += new TimeSpan(0, 0, 0, 0, shiftMs);
        return t.ToString(TimeFormat);
    }
}