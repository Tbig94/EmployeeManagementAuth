
using EmployeeManagementAuth.Models;
using EmployeeManagementAuth.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EmployeeManagementAuth.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        #region Fields
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly string EMPLOYEE_MANAGEMENT_ADMIN_EMAIL = Environment.GetEnvironmentVariable("EMPLOYEE_MANAGEMENT_ADMIN_EMAIL");
        private readonly string EMPLOYEE_MANAGEMENT_ADMIN_USERNAME = Environment.GetEnvironmentVariable("EMPLOYEE_MANAGEMENT_ADMIN_USERNAME");
        private readonly string EMPLOYEE_MANAGEMENT_ADMIN_NAME = Environment.GetEnvironmentVariable("EMPLOYEE_MANAGEMENT_ADMIN_NAME");
        private readonly string EMPLOYEE_MANAGEMENT_ADMIN_PASSWORD = Environment.GetEnvironmentVariable("EMPLOYEE_MANAGEMENT_ADMIN_PASSWORD");
        #endregion

        #region Ctor
        public DbInitializer(
                            ApplicationDbContext db,
                            UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #endregion

        #region Initialize
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (_db.Roles.Any(x => x.Name == Helper.Admin) && _db.Users.Any(x => x.Email == EMPLOYEE_MANAGEMENT_ADMIN_EMAIL))
            {
                return;
            }
            else if (_db.Roles.Any(x => x.Name == Helper.Admin))
            {
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = EMPLOYEE_MANAGEMENT_ADMIN_USERNAME,
                    Email = EMPLOYEE_MANAGEMENT_ADMIN_EMAIL,
                    Name = EMPLOYEE_MANAGEMENT_ADMIN_NAME
                }, EMPLOYEE_MANAGEMENT_ADMIN_PASSWORD).GetAwaiter().GetResult();

                ApplicationUser admin = _db.Users.FirstOrDefault(u => u.Email == EMPLOYEE_MANAGEMENT_ADMIN_EMAIL);
                _userManager.AddToRoleAsync(admin, Helper.Admin).GetAwaiter().GetResult();

                return;
            }
            else if (_db.Users.Any(x => x.Email == EMPLOYEE_MANAGEMENT_ADMIN_EMAIL))
            {
                _roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Helper.User)).GetAwaiter().GetResult();

                return;
            }

            _roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.User)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = EMPLOYEE_MANAGEMENT_ADMIN_USERNAME,
                Email = EMPLOYEE_MANAGEMENT_ADMIN_EMAIL,
                Name = EMPLOYEE_MANAGEMENT_ADMIN_NAME
            }, EMPLOYEE_MANAGEMENT_ADMIN_PASSWORD).GetAwaiter().GetResult();

            ApplicationUser admin2 = _db.Users.FirstOrDefault(u => u.Email == EMPLOYEE_MANAGEMENT_ADMIN_EMAIL);
            _userManager.AddToRoleAsync(admin2, Helper.Admin).GetAwaiter().GetResult();
        }
        #endregion
    }
}

