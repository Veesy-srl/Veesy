namespace Veesy.Domain.Constants;

public static class VeesyConstants
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

    public static List<PortfolioLayout> GetAvailableLayouts()
    {
        return Enum.GetValues(typeof(PortfolioLayout)).Cast<PortfolioLayout>().ToList();
    }
}