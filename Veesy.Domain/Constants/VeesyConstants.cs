namespace Veesy.Domain.Constants;

public class VeesyConstants
{
    public static class SubscriptionPlan
    {
        public const string Free = "Free";
        public const string Pro = "Pro";
        public const string Plus = "Plus";
        public const string Beta = "Beta";
    }
    
    public enum PortfolioLayout
    {
        Unset = 0,
        OneColumn = 1,
        TwoColumns = 2,
        ThreeColumns = 3,
        FourColumns = 4
    }
}