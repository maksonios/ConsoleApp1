using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SubtitleUtility;

public static class SubtitleModifier
{
    private const string DefaultTimeIntervalDelimiter = "-->";
    private const string TimeFormat = @"hh\:mm\:ss\,fff";
    private static readonly Regex TimeRegex = new (TimeIntervalPattern);
    private const string TimeIntervalPattern = @"(\d\d):(\d\d):(\d\d),(\d\d\d)";
    
    public static string ExecuteSubtitleShift(string input,
                                              int shiftMs, 
                                              string sourceTimeIntervalDelimiter = DefaultTimeIntervalDelimiter, 
                                              string targetTimeIntervalDelimiter = DefaultTimeIntervalDelimiter,
                                              bool isSubtitleNumberingEnabled = true)
    {

        ValidateTimeIntervalDelimiterConsistency(input, sourceTimeIntervalDelimiter);
        
        var modifiedInput = ReplaceTimeIntervalDelimiter(input, targetTimeIntervalDelimiter);
        
        var newStr = new StringBuilder();
        if (!isSubtitleNumberingEnabled)
        {
            for (int i = 0; i < modifiedInput.Length-1; i++)
            {
                var temp = Regex.IsMatch(modifiedInput[i+1], TimeIntervalPattern);
                if (temp)
                {
                    var timeline=TimeRegex.Replace(modifiedInput[i+1], m => AddTime(m, shiftMs));
                    newStr.AppendLine(timeline);
                    continue;
                }
                var temp1 = Regex.IsMatch(modifiedInput[i], TimeIntervalPattern);
                if (temp1)
                {
                    continue;
                }
                newStr.AppendLine(modifiedInput[i]);
            }
        }
        else
        {
            var result = TimeRegex.Replace(string.Join(Environment.NewLine, modifiedInput), m => AddTime(m, shiftMs));
            return result;
        }

        return newStr.ToString();
    }

    private static void ValidateTimeIntervalDelimiterConsistency(string input, string sourceTimeIntervalDelimiter)
    {
        var source = Regex.Split(input, "\r\n|\r|\n");
        for (var i = 0; i < source.Length-1; i++)
        {
            if (Regex.IsMatch(source[i], TimeIntervalPattern) && source[i].Substring(13, 3) != sourceTimeIntervalDelimiter)
                throw new InvalidDataException($"TimeIntervalDelimiterConsistency is not consistent across file. The subtitle line: #{i + 1}");
        }
    }

    private static string[] ReplaceTimeIntervalDelimiter(string input, string targetTimeIntervalDelimiter)
    {
        var source = Regex.Split(input, "\r\n|\r|\n");
        for (int i = 0; i < source.Length-1; i++)
        {
            if (Regex.IsMatch(source[i], TimeIntervalPattern))
            {
                var delimeter = source[i].Substring(13, 3);
                var temp = source[i].Replace(delimeter, targetTimeIntervalDelimiter);
                source[i] = temp;
            }
        }
        return source;
    }

    private static string AddTime(Match m, int shiftMs)
    {
        var t = TimeSpan.ParseExact(m.Value, TimeFormat, CultureInfo.InvariantCulture);
        t += new TimeSpan(0, 0, 0, 0, shiftMs);
        return t.ToString(TimeFormat);
    }
}