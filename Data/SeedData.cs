using Game.Authorization;
using Game.Data;
using Game.Models;
using Game.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


// dotnet aspnet-codegenerator razorpage -m Contact -dc ApplicationDbContext -outDir Pages\Contacts --referenceScriptLibraries
namespace Game.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@game.com");
                await EnsureRole(serviceProvider, adminID, Constants.AdministratorRole);

                SeedDB(context);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            if(userManager == null)
            {
                throw new Exception("userManager is null, abort seeding");
            }

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            if (userManager == null)
            {
                throw new Exception("userManager is null");
            }

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        public static void SeedDB(ApplicationDbContext context)
        {
            if(context.Weapons != null)
            {
                if (context.Weapons.Any())
                {
                    return;   // DB has been seeded
                }
                context.Weapons.AddRange(
                new Weapon
                {
                    Name = "Small Sword",
                    Damage = 20,
                    WeaponType = WeaponType.Warrior
                },
                new Weapon
                {
                    Name = "Big Sword",
                    Damage = 80,
                    WeaponType = WeaponType.Warrior
                },
                new Weapon
                {
                    Name = "Magic Sword",
                    Damage = 46,
                    WeaponType = WeaponType.Wizard
                }
             );
            }
            context.SaveChanges();
        }
    }
}