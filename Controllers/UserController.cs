using Game.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Game.Authorization;
using Game.ViewModels;
using Game.Utils;

namespace Game.Controllers
{
    [AuthorizeAdmin]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<IdentityUser_Roles> usersWithRoles = new List<IdentityUser_Roles>();
            var users = _context.Users.ToList();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new IdentityUser_Roles() { Roles = roles, User=user });
            }
            return View(usersWithRoles);
        }

        [HttpPost]
        public async Task<IActionResult> MakeHelper(string? Id) {
            if(Id == null) {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(Id);

            if(user == null)
            {
                return NotFound();  
            }
            await _userManager.AddToRoleAsync(user, Constants.HelperRole);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveHelper(string? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }
            await _userManager.RemoveFromRoleAsync(user, Constants.HelperRole);
            return RedirectToAction(nameof(Index));
        }
    }
}
