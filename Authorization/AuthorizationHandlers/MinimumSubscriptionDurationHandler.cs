using Microsoft.AspNetCore.Authorization;
using Game.Authorization.Requirements;
using Game.Utils;

namespace Game.Authorization.AuthorizationHandlers
{
    public class MinimumSubscriptionDurationHandler : AuthorizationHandler<MinimumSubscriptionDurationRequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumSubscriptionDurationRequirement requirement)
        {
            if(context.User == null)
            {
                return Task.CompletedTask;
            }

            // Admin has always privileges
            if(context.User.IsInRole(Constants.AdministratorRole))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // Only Helpers can have this privilege
            if(!context.User.IsInRole(Constants.HelperRole))
            {
                return Task.CompletedTask;
            }

            var targetClaim = context.User.Claims.FirstOrDefault(c => c.Type == "subscription");
            if (targetClaim == null)
            {
                return Task.CompletedTask;
            }

            var subscriptionDate = DateTime.Parse(targetClaim.Value);
            var subscriptionDuration = DateTime.Now - subscriptionDate;
            var minimumDaysRequirement = requirement.MinimumYears * 365;

            if(subscriptionDuration.Days > minimumDaysRequirement)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
