using Game.Models;
using Game.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Game.Authorization.AuthorizationHandlers
{
    public class CharacterAuthorizationHandler
                : AuthorizationHandler<OperationAuthorizationRequirement, Character>
    {
        UserManager<IdentityUser> _userManager;

        public CharacterAuthorizationHandler(UserManager<IdentityUser>
            userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Character resource)
        {

            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            switch(requirement.Name)
            {
                case "Approve Character":
                    
                    break;
            }

            // If user is Administrator, authorize
            if (context.User.IsInRole(Constants.AdministratorRole))
                context.Succeed(requirement);

            // If user is Helper, authorize only Approve Operation
            else if (context.User.IsInRole(Constants.HelperRole) && requirement.Name == Constants.ApproveCharacterOperationName)
                context.Succeed(requirement);

            // If user is owner of given character, authorize
            else if (_userManager.GetUserId(context.User) == resource.OwnerID)
                context.Succeed(requirement);


            return Task.CompletedTask;
        }
    }
}