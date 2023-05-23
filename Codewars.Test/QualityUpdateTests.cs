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
            yield return new object[] { MyItem.Of("Backstage passes", 12, 15), MyItem.Of("Backstage passes", 11, 16) };
            yield return new object[] { MyItem.Of("Backstage passes", 11, 15), MyItem.Of("Backstage passes", 10, 17) };
            yield return new object[] { MyItem.Of("Backstage passes", 6, 15), MyItem.Of("Backstage passes", 5, 18) };
            yield return new object[] { MyItem.Of("Backstage passes", 1, 15), MyItem.Of("Backstage passes", 0, 0) };
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
        new NamePatternRule("Sulfuras", 0, 0),
        new NamePatternRule("Aged Brie", -1, 1),
        new FomoRule(new NamePatternRule("Backstage passes", -1, 1)),
        new SellTimeExpiredRule(new RegularItemRule()),
    }) { }
    
    public ItemUpdater(List<IUpdateRule> updateRules)
    {
        _updateRules = updateRules;
    }

    public MyItem Update(MyItem item) => item.Update(_updateRules);
}

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
    public bool Match(MyItem myItem) => true;
    public MyItem UpdateItem(MyItem myItem) => 
        new (myItem.Name, myItem.SellIn - 1, myItem.Quality.UpdateBy(-1));

    public int QualityValue => -1;
}

public class NamePatternRule : IUpdateRule
{
    private readonly string _patternToMatch;
    private readonly int _sellInUpdate;

    public NamePatternRule(string patternToMatch, int sellInUpdate, int qualityValue)
    {
        _patternToMatch = patternToMatch;
        _sellInUpdate = sellInUpdate;
        QualityValue = qualityValue;
    }

    public int QualityValue { get; }

    public bool Match(MyItem myItem) => 
        myItem.Name.Contains(_patternToMatch);

    public MyItem UpdateItem(MyItem myItem) => 
        new (myItem.Name, myItem.SellIn + (_sellInUpdate), myItem.Quality.UpdateBy(QualityValue));

}

public interface IUpdateRule
{
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