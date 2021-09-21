using EmployeeManagementAuth.Models;
using EmployeeManagementAuth.Models.ViewModels;
using EmployeeManagementAuth.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        #region Fields
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public EmployeeController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            IEnumerable<Employee> objList = _db.Employees;      //  fetching Employees from DB

            //  Loading Department.Name of current Employee
            foreach (var obj in objList)
            {
                //  Set first Department with particular Id
                obj.Department = _db.Departments.FirstOrDefault(u => u.Id == obj.DepartmentId);
            }

            return View(objList);
        }
        #endregion

        #region Create
        //  GET-Create
        public IActionResult Create()
        {
            EmployeeVM employeeVM = new EmployeeVM()
            {
                Employee = new Employee(),
                TypeDropDown = _db.Departments.Select(i => new SelectListItem
                {
                    Text = i.DepartmentName,
                    Value = i.Id.ToString()
                })
            };

            return View(employeeVM);
        }

        //  POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(EmployeeVM employeeObj)
        {
            if (ModelState.IsValid)
            {
                var currentUserEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

                DateTime currentTime = DateTime.Now;
                string logBody = employeeObj.Employee.FirstName + " " + employeeObj.Employee.LastName;
                LoggingModel loggingModel = new LoggingModel(0, currentUserEmail, logBody, currentTime, Helper.Create, Helper.Employee);

                _db.LoggingModels.Add(loggingModel);
                _db.Employees.Add(employeeObj.Employee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employeeObj);
        }
        #endregion

        #region Delete
        //  GET-Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Employees.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //  POST-Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var currentUserEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            var employeeObj = _db.Employees.Find(id);
            if (employeeObj == null)
            {
                return NotFound();
            }

            DateTime currentTime = DateTime.Now;
            string logBody = employeeObj.FirstName + " " + employeeObj.LastName;
            LoggingModel loggingModel = new LoggingModel(0, currentUserEmail, logBody, currentTime, Helper.Delete, Helper.Employee);

            _db.LoggingModels.Add(loggingModel);
            _db.Employees.Remove(employeeObj);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        //  GET-Update
        public IActionResult Update(int? id)
        {
            EmployeeVM employeeVM = new EmployeeVM()
            {
                Employee = new Employee(),
                TypeDropDown = _db.Departments.Select(i => new SelectListItem
                {
                    Text = i.DepartmentName,
                    Value = i.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                return NotFound();
            }

            employeeVM.Employee = _db.Employees.Find(id);
            if (employeeVM.Employee == null)
            {
                return NotFound();
            }

            return View(employeeVM);
        }

        //  POST-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePost(EmployeeVM employeeVMObj)
        {
            var currentUserEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            if (ModelState.IsValid)
            {
                DateTime currentTime = DateTime.Now;
                string logBody = employeeVMObj.Employee.FirstName + " " + employeeVMObj.Employee.LastName;
                LoggingModel loggingModel = new LoggingModel(0, currentUserEmail, logBody, currentTime, Helper.Update, Helper.Employee);

                _db.LoggingModels.Add(loggingModel);
                _db.Employees.Update(employeeVMObj.Employee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(employeeVMObj);
        }
        #endregion
    }
}
