using System.Globalization;
using System.Text.RegularExpressions;
using SubtitleUtility.Interfaces;

namespace SubtitleUtility.Services;

public class SubtitleModifier : ISubtitleManager
{
    private const string TimeFormat = @"hh\:mm\:ss\,fff";
    private static readonly Regex TimeRegex = new (TimeIntervalPattern);
    private const string TimeIntervalPattern = @"(\d\d):(\d\d):(\d\d),(\d\d\d)";

    public string ExecuteSubtitleShift(string input, 
                                       int shiftMs,
                                       string sourceTimeIntervalDelimiter = Consts.DefaultTimeIntervalDelimiter, 
                                       string targetTimeIntervalDelimiter = Consts.DefaultTimeIntervalDelimiter,
                                       bool isSubtitleNumberingEnabled = true,
                                       CaseSelection variable = CaseSelection.None,
                                       string startTime = "00:00:00,000",
                                       string endTime = "20:59:59,999")
    {
        var source = Regex.Split(input, "\r\n|\r|\n");

        for (var i = 0; i < source.Length - 1; i++)
        {
            ValidateTimeIntervalDelimiterConsistency(ref source[i], sourceTimeIntervalDelimiter, i+1);
            ReplaceTimeIntervalDelimiter(ref source[i], targetTimeIntervalDelimiter);
            ExecuteSubtitleShiftWithoutNumeration(ref source[i], source[i+1], shiftMs, isSubtitleNumberingEnabled);
            SubtitleToCustomCase(ref source[i], variable);
        }
        
        source = OutputSelectedTimeCodes(source, startTime, endTime);
        return string.Join(Environment.NewLine, source.Where(x => x != null));
    }

    private static string[] OutputSelectedTimeCodes(string[] source, string startTime, string endTime)
    {
        int startLineIndex = -1;
        int endLineIndex = -1;
        for (var i = 0; i < source.Length-1; i++)
        {
            if (IsTimeInterval(source[i]) && ParseTimeLine(startTime) >= ParseTimeLine(source[i].Substring(0, 12)))
            {
                startLineIndex = i-1;
            }
    
            if (IsTimeInterval(source[i]) && ParseTimeLine(endTime) <= ParseTimeLine(source[i].Substring(source[i].Length - 12)))
            {
                endLineIndex = i+2;
                break;
            }
        }

        if (startLineIndex == -1)
            startLineIndex = 0;
        if (endLineIndex == -1)
            endLineIndex = source.Length - 1;
        
        var length = endLineIndex - startLineIndex;
        
        var newSource = new string[length];
        Array.Copy(source, startLineIndex, newSource, 0, length);
        
        return newSource;
    }

    private static void SubtitleToCustomCase(ref string source, CaseSelection toUpper)
    {
        switch (toUpper)
            {
                case CaseSelection.Upper when !IsTimeInterval(source):
                {
                    source = source.ToUpper();
                    break;
                }
                case CaseSelection.Lower when !IsTimeInterval(source):
                {
                    source = source.ToLower();
                    break;
                }
            }
    }

    private static void ExecuteSubtitleShiftWithoutNumeration(ref string source, string timeInterval, int shiftMs, bool isSubtitleNumberingEnabled)
    {
        if (!isSubtitleNumberingEnabled && IsTimeInterval(timeInterval))
        {
            source = null!;
            return;
        }
        if (IsTimeInterval(source))
        {
            var timeLine = TimeRegex.Replace(source, m => AddTime(m, shiftMs));
            source = timeLine;
        }
    }

    private static void ValidateTimeIntervalDelimiterConsistency(ref string source, string sourceTimeIntervalDelimiter, int nonconsistentLine)
    {
        if (IsTimeInterval(source) && ParseDelimiter(source) != sourceTimeIntervalDelimiter)
                throw new InvalidDataException($"TimeIntervalDelimiterConsistency is not consistent across file. The subtitle line: #{nonconsistentLine}");
    }

    private static void ReplaceTimeIntervalDelimiter(ref string source, string targetTimeIntervalDelimiter)
    {
        if (!IsTimeInterval(source))
                return;
        var delimiter = ParseDelimiter(source);
        var temp = source.Replace(delimiter, targetTimeIntervalDelimiter);
        source = temp;
    }

    private static string AddTime(Match m, int shiftMs)
    {
        var t = TimeSpan.ParseExact(m.Value, TimeFormat, CultureInfo.InvariantCulture);
        t += new TimeSpan(0, 0, 0, 0, shiftMs);
        return t.ToString(TimeFormat);
    }

    private static bool IsTimeInterval(string? input)
    {
        if (input == null)
            return false;
        return Regex.IsMatch(input, TimeIntervalPattern);
    }

    private static string ParseDelimiter(string input) => input.Substring(13, 3);

    private static TimeSpan ParseTimeLine(string input) =>
        TimeSpan.ParseExact(input, TimeFormat, CultureInfo.InvariantCulture);
}