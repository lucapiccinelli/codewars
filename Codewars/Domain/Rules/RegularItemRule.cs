namespace Codewars.Domain.Rules;

public class RegularItemRule : IUpdateRule
{
    public bool Match(MyItem myItem) => true;
    public MyItem UpdateItem(MyItem myItem) => 
        new (myItem.Name, myItem.SellIn - 1, myItem.Quality.UpdateBy(QualityValue));

    public int QualityValue => -1;
}