using Microsoft.AspNetCore.Authorization;
using Game.Utils;

namespace Game.Authorization
{
    public class AuthorizeAdmin : AuthorizeAttribute
    {
        public AuthorizeAdmin()
        {
            Roles = Constants.AdministratorRole;
        }
    }
}
