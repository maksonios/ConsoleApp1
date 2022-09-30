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
        var source = Regex.Split(input, "\r\n|\r|\n");

        ValidateTimeIntervalDelimiterConsistency(source, sourceTimeIntervalDelimiter);
        
        ReplaceTimeIntervalDelimiter(source, targetTimeIntervalDelimiter);

        return !isSubtitleNumberingEnabled ?
            ExecuteSubtitleShiftWithoutNumeration(source, shiftMs) :
            ExecuteSubtitleShiftWithNumeration(source, shiftMs);
    }

    private static string ExecuteSubtitleShiftWithoutNumeration(string[] source, int shiftMs)
    {
        var newStr = new StringBuilder();
        for (var i = 0; i < source.Length-1; i++)
        {
            var isNextLineTimeInterval = Regex.IsMatch(source[i+1], TimeIntervalPattern);
            if (isNextLineTimeInterval)
                continue;

            var isLineTimeInterval = Regex.IsMatch(source[i], TimeIntervalPattern);
            if (isLineTimeInterval)
            {
                var timeline=TimeRegex.Replace(source[i], m => AddTime(m, shiftMs));
                newStr.AppendLine(timeline);
                continue;
            }
                
            newStr.AppendLine(source[i]);
        }
        return newStr.ToString();
    }

    private static string ExecuteSubtitleShiftWithNumeration(string[] source, int shiftMs)
    {
        return TimeRegex.Replace(string.Join(Environment.NewLine, source), m => AddTime(m, shiftMs));
    }

    private static void ValidateTimeIntervalDelimiterConsistency(string[] source, string sourceTimeIntervalDelimiter)
    {
        for (var i = 0; i < source.Length-1; i++)
        {
            if (Regex.IsMatch(source[i], TimeIntervalPattern) && ParseDelimiter(source[i]) != sourceTimeIntervalDelimiter)
                throw new InvalidDataException($"TimeIntervalDelimiterConsistency is not consistent across file. The subtitle line: #{i + 1}");
        }
    }

    private static void ReplaceTimeIntervalDelimiter(string[] source, string targetTimeIntervalDelimiter)
    {
        for (int i = 0; i < source.Length-1; i++)
        {
            if (Regex.IsMatch(source[i], TimeIntervalPattern))
            {
                var delimiter = ParseDelimiter(source[i]);
                var temp = source[i].Replace(delimiter, targetTimeIntervalDelimiter);
                source[i] = temp;
            }
        }
    }

    private static string AddTime(Match m, int shiftMs)
    {
        var t = TimeSpan.ParseExact(m.Value, TimeFormat, CultureInfo.InvariantCulture);
        t += new TimeSpan(0, 0, 0, 0, shiftMs);
        return t.ToString(TimeFormat);
    }

    private static string ParseDelimiter(string input) => input.Substring(13, 3);
}