using Microsoft.AspNetCore.Authorization;

namespace Game.Authorization.Requirements
{
    public class MinimumSubscriptionDurationRequirement : IAuthorizationRequirement
    {
        public int MinimumYears { get; }

        public MinimumSubscriptionDurationRequirement(int minYears)
        {
            MinimumYears = minYears;    
        }
    }
}
