using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using Tasker.Web.Data.Entities;
using static MudBlazor.CategoryTypes;

namespace Tasker.Web.Data.Classes
{
    public class IdentitySeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public IdentitySeedData(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task InitializeRoles()
        {
            var uuser = new ApplicationUser { UserName = "FilipHraba", Email = "hraba.filip@ssakhk.cz"  };
            await _userManager.CreateAsync(uuser, "pass.word123");
            uuser = new ApplicationUser { UserName = "LiborSenar", Email = "senar.libor@ssakhk.cz" };
            await _userManager.CreateAsync(uuser, "pass.word123");
            if (!await _db.Users.AnyAsync())
            {
                IEnumerable<ApplicationUser> Users =
                [
                    new ApplicationUser
                    {
                        Email = "ozmatlik@gmail.com",
                        UserName = "FarSniper"
                    },
                ];
                foreach (var item in Users)
                {
                    var result = await _userManager.CreateAsync(item, "pass.word123");
                    Console.WriteLine($"User {item.Email} created: {result.Succeeded}");
                }
            }
            if (!await _db.Groups.AnyAsync(x => x.Name == "All"))
            {
                await _db.Groups.AddAsync(new Group { Name = "All", Id = Guid.Empty });
                await _db.SaveChangesAsync();
            }
        }
    }
}
