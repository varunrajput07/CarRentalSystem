using CarRental.DataAccess.DbInitializer;
using CarRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CarModal> CarModals { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarBooking> CarBookings { get; set; }
        public object Names { get; internal set; }
        public object CarBooking { get; internal set; }
       

        public void Retrive()
        {
            throw new NotImplementedException();
        }


    }
    //public static class UserAndRoleDataInitializer
    //{
    //    public static void SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    //    {
    //        SeedRoles(roleManager);
    //        SeedUsers(userManager);
    //    }

    //    private static void SeedUsers(UserManager<User> userManager)
    //    {
    //        if (userManager.FindByEmailAsync("varunrajput5747@gmail.com").Result == null)
    //        {
    //            User user = new User();
    //            user.UserName = "varunrajput5747@gmail.com";
    //            user.Email = "varunrajput5747@gmail.com";
    //            user.FirstName = "varun";
    //            user.LastName = "rajput";

    //            IdentityResult result = userManager.CreateAsync(user, "#v@run5747").Result;

    //            if (result.Succeeded)
    //            {
    //                userManager.AddToRoleAsync(user, "User").Wait();
    //            }
    //        }
    //    }

    //    private static void SeedRoles(RoleManager<IdentityRole> roleManager)
    //    {
    //        if (!roleManager.RoleExistsAsync("User").Result)
    //        {
    //            IdentityRole role = new IdentityRole();
    //            role.Name = "User";
    //            IdentityResult roleResult = roleManager.
    //            CreateAsync(role).Result;
    //        }
    //    }
    //}
}