namespace Codewars.Domain.Rules;

public static class UpdateRuleExtensions
{
    public static IUpdateRule SellTime(this IUpdateRule innerRule, int threshold, Func<MyItem, MyItem, IUpdateRule, MyItem> qualityUpdater) =>
        new SellTimeRule(innerRule, qualityUpdater, threshold);
    public static IUpdateRule SellTime(this IUpdateRule innerRule, int threshold = -1, int qualityMultiplier = 2) => 
        new SellTimeRule(innerRule, threshold, qualityMultiplier);
}