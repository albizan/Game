using Microsoft.AspNetCore.Identity;

namespace Game.ViewModels
{
    public class IdentityUser_Roles
    {
        public IdentityUser User { get; set; }
        public IList<string> Roles { get; set; }
    }
}
