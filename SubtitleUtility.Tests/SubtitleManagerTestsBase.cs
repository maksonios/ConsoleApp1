using FluentAssertions;
using NUnit.Framework;
using SubtitleUtility.Interfaces;

namespace SubtitleUtility.Tests;

[TestFixture]
public abstract class SubtitleManagerTestsBase
{
    protected abstract ISubtitleManager Sut { get; }

    private static readonly string Input = "1" + Environment.NewLine + 
                                           "00:02:25,396 --> 00:02:27,273" + Environment.NewLine + 
                                           "Master Kaecilius." + Environment.NewLine + 
                                           "" + Environment.NewLine + 
                                           "2" + Environment.NewLine + 
                                           "00:02:28,399 --> 00:02:32,245" + Environment.NewLine + 
                                           "That ritual will bring you only sorrow." + Environment.NewLine + 
                                           "" + Environment.NewLine + 
                                           "3" + Environment.NewLine + 
                                           "00:03:08,230 --> 00:03:09,447" + Environment.NewLine + 
                                           "Hypocrite!" + Environment.NewLine + 
                                           "" + Environment.NewLine + 
                                           "4" + Environment.NewLine + 
                                           "00:05:05,847 --> 00:05:07,190" + Environment.NewLine + 
                                           "Challenge round, Billy." + Environment.NewLine;
    private const int ShiftMs = 500;
    private const bool IsNumerationEnabled = true;
    private const string DefaultTimeIntervalDelimiter = "-->";

    [Test]
    public void ExecuteSubtitleShift_ShouldShiftSubtitlesForward_WhenShiftParamIsPositive()
    {
        var expectedValue = "1" + Environment.NewLine + 
                            "00:02:25,896 --> 00:02:27,773" + Environment.NewLine + 
                            "Master Kaecilius." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "2" + Environment.NewLine + 
                            "00:02:28,899 --> 00:02:32,745" + Environment.NewLine + 
                            "That ritual will bring you only sorrow." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "3" + Environment.NewLine + 
                            "00:03:08,730 --> 00:03:09,947" + Environment.NewLine + 
                            "Hypocrite!" + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "4" + Environment.NewLine + 
                            "00:05:06,347 --> 00:05:07,690" + Environment.NewLine + 
                            "Challenge round, Billy.";

        var actualValue = Sut.ExecuteSubtitleShift(Input, ShiftMs);

        actualValue.Should().BeEquivalentTo(expectedValue);
    }
    
    [Test]
    public void ExecuteSubtitleShift_ShouldShiftSubtitlesBackward_WhenShiftParamIsNegative()
    {
        var expectedValue = "1" + Environment.NewLine + 
                            "00:02:24,896 --> 00:02:26,773" + Environment.NewLine + 
                            "Master Kaecilius." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "2" + Environment.NewLine + 
                            "00:02:27,899 --> 00:02:31,745" + Environment.NewLine + 
                            "That ritual will bring you only sorrow." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "3" + Environment.NewLine + 
                            "00:03:07,730 --> 00:03:08,947" + Environment.NewLine + 
                            "Hypocrite!" + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "4" + Environment.NewLine + 
                            "00:05:05,347 --> 00:05:06,690" + Environment.NewLine + 
                            "Challenge round, Billy.";

        var actualValue = Sut.ExecuteSubtitleShift(Input, -ShiftMs);

        actualValue.Should().BeEquivalentTo(expectedValue);
    }

    [Test]
    public void ExecuteSubtitleShift_ShouldEnableSubtitleNumeration_WhenBoolParamIsTrue()
    {
        var expectedValue = "1" + Environment.NewLine + 
                            "00:02:24,896 --> 00:02:26,773" + Environment.NewLine + 
                            "Master Kaecilius." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "2" + Environment.NewLine + 
                            "00:02:27,899 --> 00:02:31,745" + Environment.NewLine + 
                            "That ritual will bring you only sorrow." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "3" + Environment.NewLine + 
                            "00:03:07,730 --> 00:03:08,947" + Environment.NewLine + 
                            "Hypocrite!" + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "4" + Environment.NewLine + 
                            "00:05:05,347 --> 00:05:06,690" + Environment.NewLine + 
                            "Challenge round, Billy.";

        var actualValue = Sut.ExecuteSubtitleShift(Input, -ShiftMs);

        actualValue.Should().BeEquivalentTo(expectedValue);
    }
    
    [Test]
    public void ExecuteSubtitleShift_ShouldDisableSubtitleNumeration_WhenBoolParamIsFalse()
    {
        var expectedValue = "00:02:25,396 --> 00:02:27,273" + Environment.NewLine + 
                            "Master Kaecilius." + Environment.NewLine + 
                            "" + Environment.NewLine +
                            "00:02:28,399 --> 00:02:32,245" + Environment.NewLine + 
                            "That ritual will bring you only sorrow." + Environment.NewLine + 
                            "" + Environment.NewLine +
                            "00:03:08,230 --> 00:03:09,447" + Environment.NewLine + 
                            "Hypocrite!" + Environment.NewLine + 
                            "" + Environment.NewLine +
                            "00:05:05,847 --> 00:05:07,190" + Environment.NewLine + 
                            "Challenge round, Billy."; 


        var actualValue = Sut.ExecuteSubtitleShift(Input, 
                                                        shiftMs: 0,
                                                        DefaultTimeIntervalDelimiter,
                                                        DefaultTimeIntervalDelimiter,
                                                        !IsNumerationEnabled);

        actualValue.Should().BeEquivalentTo(expectedValue);
    }

    [Test]
    public void ExecuteSubtitleShift_ShouldThrowInvalidDataException_WhenDelimiterIsInconsistent()
    {
        var value = "1" + Environment.NewLine + 
                    "00:02:24,896 -> 00:02:26,773" + Environment.NewLine +
                    "Master Kaecilius." + Environment.NewLine +
                    "" + Environment.NewLine +
                    "2" + Environment.NewLine +
                    "00:02:27,899 --> 00:02:31,745" + Environment.NewLine +
                    "That ritual will bring you only sorrow." + Environment.NewLine +
                    "" + Environment.NewLine +
                    "3" + Environment.NewLine +
                    "00:03:07,730 --> 00:03:08,947" + Environment.NewLine +
                    "Hypocrite!" + Environment.NewLine +
                    "" + Environment.NewLine +
                    "4" + Environment.NewLine +
                    "00:05:05,347 --> 00:05:06,690" + Environment.NewLine +
                    "Challenge round, Billy.";
        
        Assert.Throws<InvalidDataException>(() => Sut.ExecuteSubtitleShift(value, -ShiftMs));
    }

    [Test]
    public void ExecuteSubtitleShift_ShouldReplaceTimeIntervalDelimiter_WhenParamIsAssigned()
    {
        var expectedValue = "1" + Environment.NewLine + 
                            "00:02:24,896 kurwa 00:02:26,773" + Environment.NewLine + 
                            "Master Kaecilius." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "2" + Environment.NewLine + 
                            "00:02:27,899 kurwa 00:02:31,745" + Environment.NewLine + 
                            "That ritual will bring you only sorrow." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "3" + Environment.NewLine + 
                            "00:03:07,730 kurwa 00:03:08,947" + Environment.NewLine + 
                            "Hypocrite!" + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "4" + Environment.NewLine + 
                            "00:05:05,347 kurwa 00:05:06,690" + Environment.NewLine + 
                            "Challenge round, Billy.";
        
        var actualValue = Sut.ExecuteSubtitleShift(Input, 
                                                        -ShiftMs,
                                                        sourceTimeIntervalDelimiter: DefaultTimeIntervalDelimiter,
                                                        targetTimeIntervalDelimiter: "kurwa");
        actualValue.Should().BeEquivalentTo(expectedValue);
    }

    [Test]
    public void ExecuteSubtitleShift_ShouldConvertStringToUpperCase()
    {
        var expectedValue = "1" + Environment.NewLine + 
                            "00:02:25,396 kurwa 00:02:27,273" + Environment.NewLine + 
                            "MASTER KAECILIUS." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "2" + Environment.NewLine + 
                            "00:02:28,399 kurwa 00:02:32,245" + Environment.NewLine + 
                            "THAT RITUAL WILL BRING YOU ONLY SORROW." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "3" + Environment.NewLine + 
                            "00:03:08,230 kurwa 00:03:09,447" + Environment.NewLine + 
                            "HYPOCRITE!" + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "4" + Environment.NewLine + 
                            "00:05:05,847 kurwa 00:05:07,190" + Environment.NewLine + 
                            "CHALLENGE ROUND, BILLY.";
        

        var actualValue = Sut.ExecuteSubtitleShift(Input, 
                                                        shiftMs: 0,
                                                        sourceTimeIntervalDelimiter: DefaultTimeIntervalDelimiter,
                                                        targetTimeIntervalDelimiter: "kurwa",
                                                        isSubtitleNumberingEnabled: true,
                                                        CaseSelection.Upper);
        actualValue.Should().BeEquivalentTo(expectedValue);
    }
    
    [Test]
    public void ExecuteSubtitleShift_ShouldConvertStringToLowerCase()
    {
        var expectedValue = "1" + Environment.NewLine + 
                            "00:02:25,396 KURWA 00:02:27,273" + Environment.NewLine + 
                            "master kaecilius." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "2" + Environment.NewLine + 
                            "00:02:28,399 KURWA 00:02:32,245" + Environment.NewLine + 
                            "that ritual will bring you only sorrow." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "3" + Environment.NewLine + 
                            "00:03:08,230 KURWA 00:03:09,447" + Environment.NewLine + 
                            "hypocrite!" + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "4" + Environment.NewLine + 
                            "00:05:05,847 KURWA 00:05:07,190" + Environment.NewLine + 
                            "challenge round, billy.";
        
        var actualValue = Sut.ExecuteSubtitleShift(Input,
                                                        shiftMs: 0, 
                                                        DefaultTimeIntervalDelimiter,
                                                        "KURWA",
                                                        true,
                                                        CaseSelection.Lower);
        actualValue.Should().BeEquivalentTo(expectedValue);
    }

    [Test]
    public void ExecuteSubtitleShift_ShouldNotConvertStringToAnyCase()
    {
        var expectedValue = "1" + Environment.NewLine + 
                            "00:02:25,396 --> 00:02:27,273" + Environment.NewLine + 
                            "Master Kaecilius." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "2" + Environment.NewLine + 
                            "00:02:28,399 --> 00:02:32,245" + Environment.NewLine + 
                            "That ritual will bring you only sorrow." + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "3" + Environment.NewLine + 
                            "00:03:08,230 --> 00:03:09,447" + Environment.NewLine + 
                            "Hypocrite!" + Environment.NewLine + 
                            "" + Environment.NewLine + 
                            "4" + Environment.NewLine + 
                            "00:05:05,847 --> 00:05:07,190" + Environment.NewLine + 
                            "Challenge round, Billy.";
        
        var actualValue = Sut.ExecuteSubtitleShift(Input, 0);
        
        actualValue.Should().BeEquivalentTo(expectedValue);
    }
    
    [Test]
    public void ExecuteSubtitleShift_ShouldCutRedundantTimeCodes()
    {
        var input = File.ReadAllText(@"C:\Users\Maksym\Desktop\temp1\Doctor.Strange.2016.720p.BluRay.x264.[YTS.MX]-English.srt");

        var expectedValue = "23" + Environment.NewLine +
                            "00:05:48,932 --> 00:05:50,149" + Environment.NewLine +
                            "Oh, I got this, Stephen." + Environment.NewLine +
                            "" + Environment.NewLine +
                            "24" + Environment.NewLine +
                            "00:05:50,225 --> 00:05:52,102" + Environment.NewLine +
                            "You've done your bit." + Environment.NewLine +
                            "Go ahead. We'll close up." + Environment.NewLine +
                            "" + Environment.NewLine +
                            "25" + Environment.NewLine +
                            "00:05:53,186 --> 00:05:54,187" + Environment.NewLine +
                            "What is it?" + Environment.NewLine +
                            "" + Environment.NewLine +
                            "26" + Environment.NewLine +
                            "00:05:54,271 --> 00:05:55,443" + Environment.NewLine +
                            "GSW.";

        var actualValue = Sut.ExecuteSubtitleShift(input, 
                                                        0,
                                                        DefaultTimeIntervalDelimiter,
                                                        DefaultTimeIntervalDelimiter,
                                                        true,
                                                        CaseSelection.None,
                                                        "00:05:48,932",
                                                        "00:05:55,443");

        actualValue.Should().BeEquivalentTo(expectedValue);
    }

}