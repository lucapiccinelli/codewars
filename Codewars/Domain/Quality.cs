namespace Codewars.Domain;

public record Quality
{
    private readonly int _lowerBound;
    private readonly int _upperBound;
    public int Value { get; }

    private Quality(int value, int lowerBound, int upperBound)
    {
        _lowerBound = lowerBound;
        _upperBound = upperBound;
        Value = value;
    }
    
    public static Quality Of(int qualityValue, int lowerBound = 0, int upperBound = 50)
    {
        if (qualityValue < lowerBound || qualityValue > upperBound) 
            throw new ArgumentException($"qualityValue should not be less the {lowerBound} and more than {upperBound}");
        
        return new Quality(qualityValue, lowerBound, upperBound);
    }
    
    public Quality UpdateBy(int qualityUpdate)
    {
        int qualityValue = Value + qualityUpdate;
        if (qualityValue < 0)
        {
            qualityValue = 0;
        }
        return Of(qualityValue, _lowerBound, _upperBound);
    }
}