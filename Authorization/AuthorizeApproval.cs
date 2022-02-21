using Microsoft.AspNetCore.Authorization;
using Game.Utils;

namespace Game.Authorization
{
    public class AuthorizeApproval : AuthorizeAttribute
    {
        public AuthorizeApproval()
        {
            Roles = $"{Constants.AdministratorRole},{Constants.HelperRole}";
        }
    }
}