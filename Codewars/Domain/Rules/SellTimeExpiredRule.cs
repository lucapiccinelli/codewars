namespace Codewars.Domain.Rules;

public class SellTimeExpiredRule : IUpdateRule
{
    private readonly IUpdateRule _innerRule;

    public SellTimeExpiredRule(IUpdateRule innerRule)
    {
        _innerRule = innerRule;
    }

    public bool Match(MyItem myItem) => _innerRule.Match(myItem);

    public MyItem UpdateItem(MyItem myItem)
    {
        MyItem updatedItem = _innerRule.UpdateItem(myItem);
        return updatedItem.SellIn < 0
            ? updatedItem with { Quality = myItem.Quality.UpdateBy(_innerRule.QualityValue * 2) }
            : updatedItem;
    }

    public int QualityValue => _innerRule.QualityValue;
}