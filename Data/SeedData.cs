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
        public static async Task Initialize(IServiceProvider serviceProvider, string password)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
                var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

                if(roleManager == null || userManager == null)
                {
                    throw new Exception("roleManager / userManager is null, aboring seed");
                }

                // Create Administrator and Helper Roles
                await CreateRole(roleManager, Constants.AdministratorRole);
                await CreateRole(roleManager, Constants.HelperRole);

                
                IdentityUser user;

                // Create Admin
                user = await CreateUser(userManager, password, "administrator@game.com");
                await AddRoleToUser(userManager, Constants.AdministratorRole, user);

                // Create Helper
                user = await CreateUser(userManager, password, "helper@game.com");
                await AddRoleToUser(userManager, Constants.HelperRole, user);

                SeedDbWithData(context);
            }
        }

        private static async Task<IdentityUser> CreateUser(UserManager<IdentityUser> userManager, string password, string userName)
        {
            if(userManager == null)
            {
                throw new Exception("userManager is null, abort seeding");
            }

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, password);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user;
        }

        private static async Task<IdentityRole> CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            var role = await roleManager.FindByNameAsync(roleName);
            if(role == null)
            {
                role = new IdentityRole(roleName);
                await roleManager.CreateAsync(role);
            }

            return role;
        }

        private static async Task AddRoleToUser(UserManager<IdentityUser> userManager, string roleName, IdentityUser user) {
            await userManager.AddToRoleAsync(user, roleName);
        }
        
        public static void SeedDbWithData(ApplicationDbContext db)
        {
            if (db.Weapons == null)
                throw new Exception("Weapons is null, abort seed");

            // If weapons table is empty, insert initial data
            if(!db.Weapons.Any())
            {
                db.Weapons.AddRange(
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

            db.SaveChanges();
        }
    }
}