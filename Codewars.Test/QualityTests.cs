using Codewars.Domain;

namespace Codewars.Test;

public class QualityTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(51)]
    public void QualityConstraintsTest(int qualityValue)
    {
        Assert.Throws<ArgumentException>(() => Quality.Of(qualityValue));
    }
}