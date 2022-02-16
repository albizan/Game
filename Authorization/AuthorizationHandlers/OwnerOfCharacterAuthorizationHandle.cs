using Game.Models;
using Game.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Game.Authorization.AuthorizationHandlers
{
    public class OwnerOfCharacterAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Character>
    {
        UserManager<IdentityUser> _userManager;

        public OwnerOfCharacterAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // Check if User making the request is the owner of the given resource
        // I can find the UserId in the context with _userManager.GetUserId(context.User)
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Character resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            if (_userManager.GetUserId(context.User) == resource.OwnerID)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}