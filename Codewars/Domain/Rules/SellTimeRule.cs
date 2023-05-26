namespace Codewars.Domain.Rules;

public class SellTimeRule : IUpdateRule
{
    private readonly IUpdateRule _innerRule;
    private readonly Func<MyItem, MyItem, IUpdateRule, MyItem> _qualityUpdater;
    private readonly int _threshold;

    public SellTimeRule(IUpdateRule innerRule, int threshold = -1, int qualityMultiplier = 2)
        : this(
            innerRule, 
            (updatedItem, item, rule) => updatedItem with { Quality = item.Quality.UpdateBy(rule.QualityValue * qualityMultiplier) }, 
            threshold)
    {
    }

    public SellTimeRule(IUpdateRule innerRule, Func<MyItem, MyItem, IUpdateRule, MyItem> qualityUpdater, int threshold = -1)
    {
        _innerRule = innerRule;
        _qualityUpdater = qualityUpdater;
        _threshold = threshold;
    }

    public bool Match(MyItem myItem) => _innerRule.Match(myItem);

    public MyItem UpdateItem(MyItem myItem)
    {
        MyItem updatedItem = _innerRule.UpdateItem(myItem);
        return updatedItem.SellIn <= _threshold
            ? _qualityUpdater(updatedItem, myItem, _innerRule)
            : updatedItem;
    }

    public int QualityValue => _innerRule.QualityValue;
}