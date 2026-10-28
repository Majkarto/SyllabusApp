using SyllabusApp.Application.Validation;

namespace SyllabusApp.Tests;

public class SyllabusRulesTests
{
    [Fact]
    public void ValidateHours_ReturnsNoErrors_ForCorrectData()
    {
        var errors = SyllabusRules.ValidateHours(ects: 5, lectureHours: 30, exerciseHours: 15, laboratoryHours: 15);
        Assert.Empty(errors);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(31)]
    public void ValidateHours_RejectsEctsOutOfRange(int ects)
    {
        var errors = SyllabusRules.ValidateHours(ects, 30, 0, 0);
        Assert.Contains(errors, e => e.Contains("ECTS"));
    }

    [Fact]
    public void ValidateHours_RejectsNegativeHours()
    {
        var errors = SyllabusRules.ValidateHours(5, -1, 30, 0);
        Assert.Contains(errors, e => e.Contains("ujemna"));
    }

    [Fact]
    public void ValidateHours_RejectsZeroHours()
    {
        var errors = SyllabusRules.ValidateHours(5, 0, 0, 0);
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void ValidateHours_RejectsMoreContactHoursThanEctsAllows()
    {
        var errors = SyllabusRules.ValidateHours(2, 60, 30, 0);
        Assert.Contains(errors, e => e.Contains("przekraczają"));
    }

    [Theory]
    [InlineData(4, 100, true)]
    [InlineData(4, 120, true)]
    [InlineData(4, 99, false)]
    [InlineData(4, 121, false)]
    public void CheckWorkload_MatchesEctsRange(int ects, int hours, bool ok)
    {
        var warning = SyllabusRules.CheckWorkload(ects, hours);
        Assert.Equal(ok, warning == null);
    }
}
