#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Game.Data;
using Game.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Game.Utils;
using Game.Authorization.AuthorizationHandlers;
using System.Security.Claims;
using Game.Authorization;

namespace Game.Controllers
{
    [Authorize]
    public class CharacterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<IdentityUser> _userManager;

        public CharacterController(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        // GET: /Character -> Retreive all characters
        public async Task<IActionResult> Index()
        {
            IQueryable<Character> characters;

            if (User == null)
            {
                return NotFound();
            }

            // Get all roles for the User
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = User.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == roleClaimType).Select(c => c.Value).ToList();
            if (roles.Contains(Constants.AdministratorRole) || roles.Contains(Constants.HelperRole))
            {
                characters = _context.Characters.Include(c => c.Weapon);
            } else {
                characters = _context.Characters.Include(c => c.Weapon).Where(c => c.OwnerID == _userManager.GetUserId(User));
            }

            ViewBag.Roles = roles;
            return View(await characters.ToListAsync());
        }

        // GET: Character/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters.Include(c => c.Weapon).FirstOrDefaultAsync(m => m.Id == id);

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, character, CharacterOperations.Read);

            // If user sending request owns the resource OR user sending request is Admin
            //if (User.IsInRole(Constants.AdministratorRole) || character.OwnerID == user.Id)
            if (isAuthorized.Succeeded) {
                ViewBag.TotalDamage = character.Damage + character.Weapon?.Damage;
                return View(character);
            }
            else {
                return NotFound();
            }

            
        }

        // GET: Character/Create
        public IActionResult Create()
        {
            // Create the select dropdown modal with value=Id and text=Name
            ViewData["Weapons"] = new SelectList(_context.Weapons, "Id", "Name");
            return View();
        }

        // POST: Character/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Damage,Type,WeaponId")] Character character)
        {
            if (ModelState.IsValid)
            {
                // Set OwnerId to newly created character
                character.OwnerID = _userManager.GetUserId(User);
                _context.Add(character);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Weapons"] = new SelectList(_context.Weapons, "Id", "Name");
            return View(character);
        }

        // GET: Character/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, character, CharacterOperations.Update);
            if(isAuthorized.Succeeded)
            {
                ViewData["WeaponId"] = new SelectList(_context.Weapons, "Id", "Id", character.WeaponId);
                ViewData["Weapons"] = new SelectList(_context.Weapons, "Id", "Name");
                return View(character);
            } else
            {
                return NotFound();
            }

            
        }

        // POST: Character/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerID,Name,Damage,Type,WeaponId")] Character character)
        {
            if (id != character.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var isAuthorized = await _authorizationService.AuthorizeAsync(User, character, CharacterOperations.Update);
                    if (isAuthorized.Succeeded)
                    {
                        _context.Update(character);
                        await _context.SaveChangesAsync();
                    } else
                    {
                        return NotFound();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterExists(character.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["WeaponId"] = new SelectList(_context.Weapons, "Id", "Name");
            return View(character);
        }

        // GET: Character/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters.Include(c => c.Weapon).FirstOrDefaultAsync(m => m.Id == id);

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, character, CharacterOperations.Delete);

            if (isAuthorized.Succeeded)
                return View(character);
            else
                return NotFound();

        }

        // POST: Character/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var character = await _context.Characters.FindAsync(id);

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, character, CharacterOperations.Delete);

            if (!isAuthorized.Succeeded) return NotFound();

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Character/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeApproval]
        public async Task<IActionResult> Approve(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if(character == null)
                return NotFound();

            character.IsApproved = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterExists(int id)
        {
            return _context.Characters.Any(e => e.Id == id);
        }
    }
}
