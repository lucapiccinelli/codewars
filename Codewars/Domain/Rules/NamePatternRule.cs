namespace Codewars.Domain.Rules;

public class NamePatternRule : IUpdateRule
{
    private readonly string _patternToMatch;
    private readonly int _sellInUpdate;

    public NamePatternRule(string patternToMatch, int sellInUpdate, int qualityValue)
    {
        _patternToMatch = patternToMatch;
        _sellInUpdate = sellInUpdate;
        QualityValue = qualityValue;
    }

    public int QualityValue { get; }

    public bool Match(MyItem myItem) => 
        myItem.Name.Contains(_patternToMatch);

    public MyItem UpdateItem(MyItem myItem) => 
        new (myItem.Name, myItem.SellIn + (_sellInUpdate), myItem.Quality.UpdateBy(QualityValue));

}