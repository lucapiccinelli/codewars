namespace Codewars.Domain.Rules;

public class FomoRule : IUpdateRule
{
    private readonly IUpdateRule _innerRule;

    public FomoRule(IUpdateRule innerRule)
    {
        _innerRule = innerRule;
    }

    public bool Match(MyItem myItem) => 
        _innerRule.Match(myItem);

    public MyItem UpdateItem(MyItem myItem)
    {
        MyItem updatedItem = _innerRule.UpdateItem(myItem);
        return updatedItem.SellIn switch
        {
            <= 0 => updatedItem with { Quality = Quality.Of(0) },
            <= 5 => UpdateQuality(myItem, updatedItem, 3),
            <= 10 => UpdateQuality(myItem, updatedItem, 2),
            _ => updatedItem
        };

    }

    private MyItem UpdateQuality(MyItem myItem, MyItem updatedItem, int multiplier) => 
        updatedItem with { Quality = myItem.Quality.UpdateBy(_innerRule.QualityValue * multiplier) };

    public int QualityValue => _innerRule.QualityValue;
}