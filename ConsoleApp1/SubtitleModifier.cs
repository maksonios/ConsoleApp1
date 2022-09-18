using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ConsoleApp1;

public static class SubtitleModifier
{
    private const string TimeFormat = @"hh\:mm\:ss\,fff";
    private static readonly Regex TimeRegex = new(@"(\d\d):(\d\d):(\d\d),(\d\d\d)");
    
    private static string AddTime(Match m, int shiftMs)
    {
        var t = TimeSpan.ParseExact(m.Value, TimeFormat, CultureInfo.InvariantCulture);
        t += new TimeSpan(0, 0, 0, 0, shiftMs);
        return t.ToString(TimeFormat);
    }
    
    public static string ExecuteSubtitleShift(string source, int shiftMs)
    {
        var result = TimeRegex.Replace(source, m => AddTime(m, shiftMs));
        return result;
    }
    
    /*public static void ExecuteSubtitleShift(string path, string newPath, int shiftMs)
    {
        var input = File.ReadAllText(path);
        input = TimeRegex.Replace(input, m => AddTime(m, shiftMs));
        File.WriteAllText(newPath, input);
    }*/
}