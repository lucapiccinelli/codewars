using Codewars.Domain.Rules;

namespace Codewars.Domain;

public record MyItem(string Name, int SellIn, Quality Quality)
{
    public static MyItem Sulfuras(int sellIn) => 
        new("Sulfuras", sellIn, Quality.Of(80, 0, 80));

    public MyItem Update(List<IUpdateRule> updateRules) =>
        updateRules
            .First(rule => rule.Match(this))
            .UpdateItem(this);

    public static MyItem Of(string name, int sellIn, int quality) => 
        new(name, sellIn, Quality.Of(quality, 0, 50));
    
    public static MyItem Of(string name, int sellIn, Quality quality) => 
        new(name, sellIn, quality);
}