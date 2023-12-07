namespace Veesy.Domain.Models;

public class MyUserSubscriptionPlan : TrackableEntity
{
    public Guid Id { get; set; }
    public string MyUserId { get; set; }
    
    public MyUser MyUser { get; set; }
    public SubscriptionPlan SubscriptionPlan { get; set; }
    public Guid SubscriptionPlanId { get; set; }
}