namespace Codewars.Domain;

public record MyItem(string Name, int SellIn, Quality Quality)
{
    public static MyItem Sulfuras(int sellIn) => 
        new("Sulfuras", sellIn, Quality.Of(80, 0, 80));

    public static MyItem Of(string name, int sellIn, int quality) => 
        new(name, sellIn, Quality.Of(quality, 0, 50));
}