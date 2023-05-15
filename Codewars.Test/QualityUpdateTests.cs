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
        MyItem updatedItem = item.Update();
        Assert.Equal(expectedItem, updatedItem);
    }
}

public record MyItem(string Name, int SellIn, Quality Quality)
{
    public MyItem Update()
    {
        int updatedSellIn = SellIn - 1;
        int qualityUpdate = -1;
        if (updatedSellIn < 0)
        {
            qualityUpdate *= 2;
        }

        if (Name.Contains("Aged Brie"))
        {
            qualityUpdate = 1;
        }

        var updatedQuality = Quality.UpdateBy(qualityUpdate);
        return new(Name, updatedSellIn, updatedQuality);
    }

    public static MyItem Of(string name, int sellIn, int quality) => 
        new(name, sellIn, Quality.Of(quality));
}