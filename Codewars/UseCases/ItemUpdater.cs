using Codewars.Domain;
using Codewars.Domain.Rules;

namespace Codewars.UseCases;

public class ItemUpdater
{
    private readonly List<IUpdateRule> _updateRules;
    public ItemUpdater() : this(new List<IUpdateRule>
    {
        new NamePatternRule("Sulfuras", 0, 0),
        new NamePatternRule("Aged Brie", -1, 1),
        new NamePatternRule("Backstage passes", -1, 1)
            .SellTime(10, 2)
            .SellTime(5, 3)
            .SellTime(0, (updatedItem, _, _) => updatedItem with { Quality = Quality.Of(0) }),
        new NamePatternRule("Conjured", -1, -2).SellTime(),
        new RegularItemRule().SellTime(),
    }) { }
    
    public ItemUpdater(List<IUpdateRule> updateRules)
    {
        _updateRules = updateRules;
    }

    public MyItem Update(MyItem item) => item.Update(_updateRules);
}