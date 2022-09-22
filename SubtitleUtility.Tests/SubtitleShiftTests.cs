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
        
        // Assert.Throws<InvalidDataException>(() => 
        //     SubtitleModifier.ExecuteSubtitleShift(Input, 500, sourceTimeIntervalDelimiter: "="));
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
        var expecetedValue 
    }
}