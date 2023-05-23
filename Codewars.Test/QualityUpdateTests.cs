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
            yield return new object[] { MyItem.Of("Sulfuras", 10, 40), MyItem.Of("Sulfuras", 10, 40) };
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
        new GetsBetterAsTimePassesRule(),
        new SellTimeExpiredRule(new RegularItemRule()),
    }) { }
    
    public ItemUpdater(List<IUpdateRule> updateRules)
    {
        _updateRules = updateRules;
    }

    public MyItem Update(MyItem item) => item.Update(_updateRules);
}

public record MyItem(string Name, int SellIn, Quality Quality)
{
    public MyItem Update(List<IUpdateRule> updateRules) =>
        updateRules
            .First(rule => rule.Match(this))
            .UpdateItem(this);

    public static MyItem Of(string name, int sellIn, int quality) => 
        new(name, sellIn, Quality.Of(quality));
}

public class RegularItemRule : IUpdateRule
{
    public int GetQualityValue(MyItem updatedSellIn, int qualityUpdate) => -1;
    public bool Match(MyItem myItem) => true;
    public MyItem UpdateItem(MyItem myItem) => 
        new (myItem.Name, myItem.SellIn - 1, myItem.Quality.UpdateBy(-1));

    public int QualityValue => -1;
}

public class GetsBetterAsTimePassesRule : IUpdateRule
{
    public int GetQualityValue(MyItem item, int qualityUpdate)
    {
        if (Match(item))
        {
            qualityUpdate = 1;
        }
        return qualityUpdate;
    }

    public bool Match(MyItem myItem) => 
        myItem.Name.Contains("Aged Brie");

    public MyItem UpdateItem(MyItem myItem) => 
        new (myItem.Name, myItem.SellIn - 1, myItem.Quality.UpdateBy(1));

    public int QualityValue => 1;
}

public interface IUpdateRule
{
    public int GetQualityValue(MyItem updatedSellIn, int qualityUpdate);
    bool Match(MyItem myItem);
    MyItem UpdateItem(MyItem myItem);
    int QualityValue { get; }
}

public class SellTimeExpiredRule : IUpdateRule
{
    private readonly IUpdateRule _innerRule;

    public SellTimeExpiredRule(IUpdateRule innerRule)
    {
        _innerRule = innerRule;
    }

    public int GetQualityValue(MyItem item, int qualityUpdate)
    {
        if (item.SellIn < 0)
        {
            qualityUpdate *= 2;
        }
        return qualityUpdate;
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