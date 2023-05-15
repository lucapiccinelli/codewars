using System.Collections;
using Codewars.Domain;

namespace Codewars.Test;

public class QualityUpdateTests
{
    public class TestItemGenerator: IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { MyItem.Of("Banana", 10, 5), MyItem.Of("Banana", 9, 4) };
            yield return new object[] { MyItem.Of("Banana", 0, 5), MyItem.Of("Banana", -1, 3) };
            yield return new object[] { MyItem.Of("Banana", 10, 0), MyItem.Of("Banana", 9, 0) };
            yield return new object[] { MyItem.Of("Aged Brie", 10, 5), MyItem.Of("Aged Brie", 9, 6) };
        }

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();
    }
    
    [Theory]
    [ClassData(typeof(TestItemGenerator))]
    public void QualityOfAnItem_ShouldBeUpdated_AsDaysPasses(MyItem item, MyItem expectedItem)
    {
        ItemUpdater itemUpdater = new ItemUpdater();
        MyItem updatedItem = itemUpdater.Update(item);
        Assert.Equal(expectedItem, updatedItem);
    }
}

public class ItemUpdater
{
    private readonly List<IUpdateRule> _updateRules;
    public ItemUpdater() : this(new List<IUpdateRule>
    {
        new RegularItemRule(),
        new SellTimeExpiredRule(),
        new GetsBetterAsTimePassesRule(),
    }) { }
    
    public ItemUpdater(List<IUpdateRule> updateRules)
    {
        _updateRules = updateRules;
    }

    public MyItem Update(MyItem item) => item.Update(_updateRules);
}

public record MyItem(string Name, int SellIn, Quality Quality)
{
    public MyItem Update(List<IUpdateRule> updateRules)
    {
        var itemSellInUpdated = UpdateSellIn();
        var qualityUpdate = UpdateQuality(itemSellInUpdated, updateRules);

        var updatedQuality = Quality.UpdateBy(qualityUpdate);
        return new(Name, itemSellInUpdated.SellIn, updatedQuality);
    }

    private static int UpdateQuality(MyItem itemSellInUpdated, List<IUpdateRule> rules) =>
        rules
            .Aggregate(0, (current, rule) => rule.GetQualityValue(itemSellInUpdated, current));

    private MyItem UpdateSellIn() => 
        this with { SellIn = SellIn - 1 };

    public static MyItem Of(string name, int sellIn, int quality) => 
        new(name, sellIn, Quality.Of(quality));
}

public class RegularItemRule : IUpdateRule
{
    public int GetQualityValue(MyItem updatedSellIn, int qualityUpdate) => -1;
}

public class GetsBetterAsTimePassesRule : IUpdateRule
{
    public int GetQualityValue(MyItem item, int qualityUpdate)
    {
        if (item.Name.Contains("Aged Brie"))
        {
            qualityUpdate = 1;
        }
        return qualityUpdate;
    }
}

public interface IUpdateRule
{
    public int GetQualityValue(MyItem updatedSellIn, int qualityUpdate);
}

public class SellTimeExpiredRule : IUpdateRule
{
    public int GetQualityValue(MyItem item, int qualityUpdate)
    {
        if (item.SellIn < 0)
        {
            qualityUpdate *= 2;
        }
        return qualityUpdate;
    }
}