using CarRental.Data;
using CarRental.DataAccess.DbInitializer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace CarRental.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
    

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public void Initialize()
        {
            //migrations if they are not applied
            //try
            //{
            //    if (_db.Database.GetPendingMigrations().Count() > 0)
            //    {
            //        _db.Database.Migrate();
            //    }
            //}
            //catch (Exception ex)
            //{

            //}

            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
                
            }
            //_roleManager.CreateAsync(new IdentityRole("SD.Role_Admin")).GetAwaiter().GetResult();

            //_roleManager.CreateAsync(new IdentityRole("SD.Role_Employee")).GetAwaiter().GetResult();
            //_roleManager.CreateAsync(new IdentityRole("SD.Role_User_Indi")).GetAwaiter().GetResult();
            //_roleManager.CreateAsync(new IdentityRole("SD.Role_User_Comp")).GetAwaiter().GetResult();

            //if roles are not created, then we will create admin user as well
            if (_userManager.FindByEmailAsync("varunrajput5747@gmail.com").GetAwaiter().GetResult() == null)
            {
                _userManager.CreateAsync(new IdentityUser
                {
                    UserName = "varunrajput5747@gmail.com",
                    Email = "varunrajput5747@gmail.com",
                    EmailConfirmed = true,
                    //FirstName = "Varun ",
                    //LastName ="Rajput",
                    PhoneNumber = "1112223333",
                    //StreetAddress = "test 123 Ave",
                    //State = "IL",
                    //PostalCode = "23422",
                    //City = "Surat"
                }, "Admin123#").GetAwaiter().GetResult();

                var user = _db.Users.FirstOrDefault(u => u.Email == "varunrajput5747@gmail.com");
                _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
            }
            

        }
    }
}
