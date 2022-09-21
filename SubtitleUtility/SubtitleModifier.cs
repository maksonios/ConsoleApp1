using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SubtitleUtility;

public static class SubtitleModifier
{
    private const string DefaultTimeIntervalDelimiter = "-->";
    private const string TimeFormat = @"hh\:mm\:ss\,fff";
    private static readonly Regex TimeRegex = new(@"(\d\d):(\d\d):(\d\d),(\d\d\d)");
    
    public static string ExecuteSubtitleShift(string[] source,
                                              int shiftMs, 
                                              string sourceTimeIntervalDelimiter = DefaultTimeIntervalDelimiter, // TODO 
                                              string targetTimeIntervalDelimiter = DefaultTimeIntervalDelimiter, // TODO
                                              bool isSubtitleNumberingEnabled = true)
    {
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
            var result = TimeRegex.Replace(string.Join("\n",source), m => AddTime(m, shiftMs));
            return result;
        }

        return newStr.ToString();
    }
    
    private static string AddTime(Match m, int shiftMs)
    {
        var t = TimeSpan.ParseExact(m.Value, TimeFormat, CultureInfo.InvariantCulture);
        t += new TimeSpan(0, 0, 0, 0, shiftMs);
        return t.ToString(TimeFormat);
    }
    
    /*public static void ExecuteSubtitleShift(string path, string newPath, int shiftMs)
    {
        var input = File.ReadAllText(path);
        input = TimeRegex.Replace(input, m => AddTime(m, shiftMs));
        File.WriteAllText(newPath, input);
    }*/
}