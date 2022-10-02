using FluentAssertions;
using NUnit.Framework;

namespace SubtitleUtility.Tests;

[TestFixture]
public class SubtitleShiftTests
{
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
                            "Challenge round, Billy." + Environment.NewLine;

        var actualValue = SubtitleModifier.ExecuteSubtitleShift(Input, ShiftMs);

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
                            "Challenge round, Billy." + Environment.NewLine;

        var actualValue = SubtitleModifier.ExecuteSubtitleShift(Input, -ShiftMs);

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
                            "Challenge round, Billy." + Environment.NewLine;

        var actualValue = SubtitleModifier.ExecuteSubtitleShift(Input, -ShiftMs);

        actualValue.Should().BeEquivalentTo(expectedValue);
    }
    
    [Test]
    public void ExecuteSubtitleShift_ShouldDisableSubtitleNumeration_WhenBoolParamIsFalse()
    {
        var expectedValue = "00:02:24,896 --> 00:02:26,773" + Environment.NewLine + 
                            "Master Kaecilius." + Environment.NewLine + 
                            "" + Environment.NewLine +
                            "00:02:27,899 --> 00:02:31,745" + Environment.NewLine + 
                            "That ritual will bring you only sorrow." + Environment.NewLine + 
                            "" + Environment.NewLine +
                            "00:03:07,730 --> 00:03:08,947" + Environment.NewLine + 
                            "Hypocrite!" + Environment.NewLine + 
                            "" + Environment.NewLine +
                            "00:05:05,347 --> 00:05:06,690" + Environment.NewLine + 
                            "Challenge round, Billy." + Environment.NewLine; 


        var actualValue = SubtitleModifier.ExecuteSubtitleShift(Input,
                                                                -ShiftMs,
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
                    "Challenge round, Billy." + Environment.NewLine;
        
        Assert.Throws<InvalidDataException>(() => SubtitleModifier.ExecuteSubtitleShift(value, -ShiftMs));
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
                            "Challenge round, Billy." + Environment.NewLine;
        
        var actualValue = SubtitleModifier.ExecuteSubtitleShift(Input,
                                                                -ShiftMs,
                                                                DefaultTimeIntervalDelimiter,
                                                                "kurwa");
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
                            "CHALLENGE ROUND, BILLY." + Environment.NewLine;
        

        var actualValue = SubtitleModifier.ExecuteSubtitleShift(Input,
                                                                0,
                                                                DefaultTimeIntervalDelimiter,
                                                                "kurwa",
                                                                true,
                                                                SubtitleModifier.CaseSelection.Upper);
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
                            "challenge round, billy." + Environment.NewLine;
        
        var actualValue = SubtitleModifier.ExecuteSubtitleShift(Input,
                                                                0,
                                                                DefaultTimeIntervalDelimiter,
                                                                "KURWA",
                                                                true,
                                                                SubtitleModifier.CaseSelection.Lower);
        actualValue.Should().BeEquivalentTo(expectedValue);
    }

    [Test]
    public void ExecuteSubtitleShift_ShouldNotConvertStringToAnyCase()
    {
        var expectedValue = Input;
        var actualValue = SubtitleModifier.ExecuteSubtitleShift(Input, 0);
        actualValue.Should().BeEquivalentTo(expectedValue);
    }
}