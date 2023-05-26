namespace Codewars.Domain.Rules;

public class FomoRule : IUpdateRule
{
    private readonly IUpdateRule _innerRule;

    public FomoRule(IUpdateRule innerRule)
    {
        _innerRule =
            new SellTimeRule(
                new SellTimeRule(
                    new SellTimeRule(innerRule, 10, 2), 5, 3),
    (updatedItem, _, _) => updatedItem with { Quality = Quality.Of(0) }, 0
            );
    }

    public bool Match(MyItem myItem) => 
        _innerRule.Match(myItem);

    public MyItem UpdateItem(MyItem myItem) => _innerRule.UpdateItem(myItem);

    public int QualityValue => _innerRule.QualityValue;
}