using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementAuth.Utilities
{
    public class Helper
    {
        public static string Admin = "Admin";
        public static string User = "User";

        public static string Create = "create";
        public static string Update = "update";
        public static string Delete = "delete";
        public static string Employee = "employee";
        public static string Department = "department";

        public static List<SelectListItem> GetRolesForDropDown()
        {
            return new List<SelectListItem>
            {
                new SelectListItem{ Value = Helper.Admin, Text = Helper.Admin },
                new SelectListItem{ Value = Helper.Admin, Text = Helper.User }
            };
        }
    }
}
