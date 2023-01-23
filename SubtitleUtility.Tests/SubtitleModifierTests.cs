using NUnit.Framework;
using SubtitleUtility.Interfaces;
using SubtitleUtility.Services;

namespace SubtitleUtility.Tests;

[TestFixture]
public class SubtitleModifierTests : SubtitleManagerTestsBase
{
    protected override ISubtitleManager Sut => new SubtitleModifier();
}