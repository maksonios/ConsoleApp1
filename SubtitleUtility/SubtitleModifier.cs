using System.Globalization;
using System.Text.RegularExpressions;

namespace SubtitleUtility;

public static class SubtitleModifier
{
    private const string DefaultTimeIntervalDelimiter = "-->";
    private const string TimeFormat = @"hh\:mm\:ss\,fff";
    private static readonly Regex TimeRegex = new (TimeIntervalPattern);
    private const string TimeIntervalPattern = @"(\d\d):(\d\d):(\d\d),(\d\d\d)";

    public enum CaseSelection
    {
        Upper,
        Lower,
        None
    }
    
    public static string ExecuteSubtitleShift(string input,
                                              int shiftMs, 
                                              string sourceTimeIntervalDelimiter = DefaultTimeIntervalDelimiter, 
                                              string targetTimeIntervalDelimiter = DefaultTimeIntervalDelimiter,
                                              bool isSubtitleNumberingEnabled = true,
                                              CaseSelection variable = CaseSelection.None)
    {
        var source = Regex.Split(input, "\r\n|\r|\n");

        ValidateTimeIntervalDelimiterConsistency(source, sourceTimeIntervalDelimiter);
        
        ReplaceTimeIntervalDelimiter(source, targetTimeIntervalDelimiter);

        ExecuteSubtitleShiftWithoutNumeration(source, shiftMs, isSubtitleNumberingEnabled);
        
        SubtitleToCustomCase(source, variable);

        return string.Join(Environment.NewLine, source.Where(x => x != null));
    }

    private static void SubtitleToCustomCase(string[] source, CaseSelection toUpper)
    {
        for (var i = 0; i < source.Length; i++)
        {
            switch (toUpper)
            {
                case CaseSelection.Upper when !IsTimeInterval(source[i]):
                {
                    source[i] = source[i].ToUpper();
                    break;
                }
                case CaseSelection.Lower when !IsTimeInterval(source[i]):
                {
                    source[i] = source[i].ToLower();
                    break;
                }
            }
        }
    }

    private static void ExecuteSubtitleShiftWithoutNumeration(string[] source, int shiftMs, bool isSubtitleNumberingEnabled)
    {
        for (var i = 0; i < source.Length-1; i++)
        {
            if (!isSubtitleNumberingEnabled && IsTimeInterval(source[i+1]))
            {
                source[i] = null!;
                continue;
            }
            if (IsTimeInterval(source[i]))
            {
                var timeLine = TimeRegex.Replace(source[i], m => AddTime(m, shiftMs));
                source[i] = timeLine;
            }
        }
    }

    private static void ValidateTimeIntervalDelimiterConsistency(string[] source, string sourceTimeIntervalDelimiter)
    {
        for (var i = 0; i < source.Length-1; i++)
        {
            if (IsTimeInterval(source[i]) && ParseDelimiter(source[i]) != sourceTimeIntervalDelimiter)
                throw new InvalidDataException($"TimeIntervalDelimiterConsistency is not consistent across file. The subtitle line: #{i + 1}");
        }
    }

    private static void ReplaceTimeIntervalDelimiter(string[] source, string targetTimeIntervalDelimiter)
    {
        for (var i = 0; i < source.Length-1; i++)
        {
            if (!IsTimeInterval(source[i]))
                continue;
            var delimiter = ParseDelimiter(source[i]);
            var temp = source[i].Replace(delimiter, targetTimeIntervalDelimiter);
            source[i] = temp;
        }
    }

    private static string AddTime(Match m, int shiftMs)
    {
        var t = TimeSpan.ParseExact(m.Value, TimeFormat, CultureInfo.InvariantCulture);
        t += new TimeSpan(0, 0, 0, 0, shiftMs);
        return t.ToString(TimeFormat);
    }

    private static bool IsTimeInterval(string input)
    {
        return Regex.IsMatch(input, TimeIntervalPattern);
    }

    private static string ParseDelimiter(string input) => input.Substring(13, 3);
}