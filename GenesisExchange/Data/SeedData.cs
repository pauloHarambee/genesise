using GenesisExchange.Models;
using Microsoft.AspNetCore.Identity;

namespace GenesisExchange.Data
{
    public static class SeedData
    {
        public static void Populate(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                //Scoping instance of Application DbContext
                var _appDbContextService = scope.ServiceProvider.GetRequiredService<AppDbContext>;
                var _appDbContext = _appDbContextService.Invoke();

                if(!_appDbContext.Banks.Any())
                {
                    _appDbContext.AddRange(new[] {
                        new Bank {Name = "CBE (Commercial Bank of Ethiopia"},
                        new Bank {Name = "Abyssiana Bank"},
                        new Bank {Name = "Wegagen Bank"},
                        new Bank {Name = "Awash Bank"},
                        new Bank {Name = "Dashen Bank"},
                        new Bank {Name = "Oromia Bank"},
                        new Bank {Name = "Amhara Bank"}
                    });
                    _appDbContext.SaveChanges();
                }
            }
        }
        public static async Task EnsureIdentity(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                //Scoping instance of Application DbContext
                var userManagerService = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManagerService = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[]{ "Sender", "Receiver"};

                foreach (var role in roles)
                {
                    await SeedRole(roleManagerService, role.Trim());
                }

                var users = new[]
                {
                    new AppUser {
                        Email = "recuser@genesisexchange.net",
                        UserName ="Reciever"
                    },
                    new AppUser {
                        Email = "senuser@genesisexchange.net",
                        UserName ="Sender"
                    }
                };

                await SeedUser(roleManagerService, userManagerService,
                       new AppUser
                       {
                           Email = "recuser@genesisexchange.net",
                           UserName = "Reciever"
                       }, "EmW?8Rcq", "Receiver");

                await SeedUser(roleManagerService, userManagerService,
                       new AppUser
                       {
                           Email = "senuser@genesisexchange.net",
                           UserName = "Sender"
                       }, "ZU$4wkr1", "Sender");
            }
        }

        private static async Task SeedUser(RoleManager<IdentityRole> roleManagerService,
             UserManager<AppUser> userManagerService, AppUser user, string Password, string Role)
        {
            await SeedRole(roleManagerService, Role);

            if (await userManagerService
                .FindByNameAsync(user.UserName) is null)
            {
                var results = await userManagerService.CreateAsync(user, Password);
                if (results.Succeeded)
                    await userManagerService.AddToRoleAsync(user, role: Role);
            }
        }

        private static async Task SeedRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole { Name = roleName });
        }
    }
}
