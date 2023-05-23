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
        new FomoRule(new NamePatternRule("Backstage passes", -1, 1)),
        new SellTimeExpiredRule(new NamePatternRule("Conjured", -1, -2)),
        new SellTimeExpiredRule(new RegularItemRule()),
    }) { }
    
    public ItemUpdater(List<IUpdateRule> updateRules)
    {
        _updateRules = updateRules;
    }

    public MyItem Update(MyItem item) => item.Update(_updateRules);
}