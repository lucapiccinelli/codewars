using System.Collections;

namespace Codewars.Test;

public class QualityUpdateTests
{
    public class TestItemGenerator: IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new MyItem("Banana", 10, 5), new MyItem("Banana", 9, 4) };
            yield return new object[] { new MyItem("Banana", 0, 5), new MyItem("Banana", -1, 3) };
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

public record MyItem(string Name, int SellIn, int Quality)
{
    public MyItem Update()
    {
        int updatedSellIn = SellIn - 1;
        int qualityUpdate = 1;
        if (updatedSellIn < 0)
        {
            qualityUpdate *= 2;
        }
        return new(Name, updatedSellIn, Quality - qualityUpdate);
    }
}