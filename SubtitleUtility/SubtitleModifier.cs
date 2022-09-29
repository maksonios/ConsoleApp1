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
        
        if (!isSubtitleNumberingEnabled)
        {
            var newStr = new StringBuilder();
            for (var i = 0; i < modifiedInput.Length-1; i++)
            {
                var isNextLineTimeInterval = Regex.IsMatch(modifiedInput[i+1], TimeIntervalPattern);
                if (isNextLineTimeInterval)
                    continue;

                var isLineTimeInterval = Regex.IsMatch(modifiedInput[i], TimeIntervalPattern);
                if (isLineTimeInterval)
                {
                    var timeline=TimeRegex.Replace(modifiedInput[i], m => AddTime(m, shiftMs));
                    newStr.AppendLine(timeline);
                    continue;
                }
                
                newStr.AppendLine(modifiedInput[i]);
            }
            return newStr.ToString();
        }
        else
        {
            var result = TimeRegex.Replace(string.Join(Environment.NewLine, modifiedInput), m => AddTime(m, shiftMs));
            return result;
        }
    }

    private static void ValidateTimeIntervalDelimiterConsistency(string input, string sourceTimeIntervalDelimiter)
    {
        var source = Regex.Split(input, "\r\n|\r|\n");
        for (var i = 0; i < source.Length-1; i++)
        {
            if (Regex.IsMatch(source[i], TimeIntervalPattern) && ParseDelimiter(source[i]) != sourceTimeIntervalDelimiter)
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
                var delimiter = ParseDelimiter(source[i]);
                var temp = source[i].Replace(delimiter, targetTimeIntervalDelimiter);
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

    private static string ParseDelimiter(string input) => input.Substring(13, 3);
}