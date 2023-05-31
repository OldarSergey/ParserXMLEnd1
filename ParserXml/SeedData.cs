using Microsoft.AspNetCore.Identity;
using ParserXml.Data;
using ParserXml.Model.EntitiesDbContext;

namespace XMLParser
{
    public static class SeedData
    {
        public static async Task EnsureSeedData(IServiceProvider provider)
        {
            var roleMgr = provider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            foreach (var roleName in RoleNames.AllRoles)
            {
                var role = await roleMgr.FindByNameAsync(roleName);

                if (role == null)
                {
                    var result = await roleMgr.CreateAsync(new IdentityRole<int> { Name = roleName });
                    if (!result.Succeeded)
                        throw new Exception(result.Errors.First().Description);
                }
            }

            var userMgr = provider.GetRequiredService<UserManager<User>>();

            var adminUser = await userMgr.FindByEmailAsync(DefaultUsers.Administrator.Email);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    Email = DefaultUsers.Administrator.Email,
                    EmailConfirmed = true,
                    UserName = DefaultUsers.Administrator.UserName
                };

                var adminResult = await userMgr.CreateAsync(adminUser, "F7CMQ3NUs!");
                if (!adminResult.Succeeded)
                    throw new Exception(adminResult.Errors.First().Description);
            }

            await userMgr.AddToRoleAsync(adminUser, RoleNames.Administator);

            // Сохраняем изменения в базе данных
            await provider.GetRequiredService<ApplicationDbContext>().SaveChangesAsync();
        }



        public static class RoleNames
        {
            public const string Administator = "Администратор";

            public static IEnumerable<string> AllRoles
            {
                get
                {
                    yield return Administator;
                }
            }
        }

        public static class DefaultUsers
        {
            public static readonly User Administrator = new()
            {
                Email = "admin.su@gmail.com",
                EmailConfirmed = true,
                UserName = "admin.su@gmail.com"
            };

            public static IEnumerable<User> AllUsers
            {
                get
                {
                    yield return Administrator;
                }
            }
        }

    }
}