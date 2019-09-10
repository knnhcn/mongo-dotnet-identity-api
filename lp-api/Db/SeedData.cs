using lp_api.Configuration;
using lp_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace lp_api.Db
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider services)
        {
            RoleManager<UserRoleEntity> roleManager = services.GetRequiredService<RoleManager<UserRoleEntity>>();
            UserManager<UserEntity> userManager = services.GetRequiredService<UserManager<UserEntity>>();

            bool dataExists = roleManager.Roles.Any() || userManager.Users.Any();

            if (dataExists)
            {
                return;
            } else
            {
                await AddUsers(roleManager, userManager);
                await AddAdmins(roleManager, userManager);
            }

        }

        private static async Task AddUsers(RoleManager<UserRoleEntity> roleManager, UserManager<UserEntity> userManager)
        {
            await roleManager.CreateAsync(new UserRoleEntity(LpRoles.USER));

            UserEntity user = new UserEntity
            {
                Email = "user@email.com",
                UserName = "user@email.com",
                CreatedAt = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            await userManager.CreateAsync(user, "Testing12345");
            await userManager.AddToRoleAsync(user, LpRoles.USER);
            await userManager.UpdateAsync(user);

        }

        private static async Task AddAdmins(RoleManager<UserRoleEntity> roleManager, UserManager<UserEntity> userManager)
        {
            await roleManager.CreateAsync(new UserRoleEntity(LpRoles.ADMIN));

            UserEntity user = new UserEntity
            {
                Email = "admin@email.com",
                UserName = "admin@email.com",
                CreatedAt = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            await userManager.CreateAsync(user, "Testing12345");
            await userManager.AddToRoleAsync(user, LpRoles.ADMIN);
            await userManager.UpdateAsync(user);

        }
    }
}
