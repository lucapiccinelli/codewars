namespace Codewars.Domain.Rules;

public interface IUpdateRule
{
    bool Match(MyItem myItem);
    MyItem UpdateItem(MyItem myItem);
    int QualityValue { get; }
}