namespace SubtitleUtility.Interfaces;

public interface ISubtitleManager
{
    string ExecuteSubtitleShift(string input, 
                                int shiftMs, 
                                string sourceTimeIntervalDelimiter = Consts.DefaultTimeIntervalDelimiter, 
                                string targetTimeIntervalDelimiter = Consts.DefaultTimeIntervalDelimiter, 
                                bool isSubtitleNumberingEnabled = true, 
                                CaseSelection variable = CaseSelection.None, 
                                string startTime = "00:00:00,000", 
                                string endTime = "20:59:59,999");
}