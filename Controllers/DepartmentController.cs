using EmployeeManagementAuth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using EmployeeManagementAuth.Utilities;

namespace EmployeeManagement.Controllers
{
    public class DepartmentController : Controller
    {
        #region Fields
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor
        public DepartmentController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            IEnumerable<Department> objList = _db.Departments;
            return View(objList);
        }
        #endregion

        #region Create
        //  GET-Create
        public IActionResult Create()
        {
            return View();
        }
        
        //  POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(Department departmentObj)
        {
            if (ModelState.IsValid)
            {
                var currentUserEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

                Department dep = _db.Departments.FirstOrDefault(i => i.DepartmentName == departmentObj.DepartmentName);
                if (dep != null)
                {
                    return RedirectToAction("Create", "Department");
                }
                
                DateTime currentTime = DateTime.Now;
                LoggingModel loggingModel = new LoggingModel(0, currentUserEmail, departmentObj.DepartmentName, currentTime, Helper.Create, Helper.Department);

                _db.LoggingModels.Add(loggingModel);
                _db.Departments.Add(departmentObj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(departmentObj);
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

            var obj = _db.Departments.Find(id);
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

            var departmentObj = _db.Departments.Find(id);
            if (departmentObj == null)
            {
                return NotFound();
            }

            DateTime currentTime = DateTime.Now;
            LoggingModel loggingModel = new LoggingModel(0, currentUserEmail, departmentObj.DepartmentName, currentTime, Helper.Delete, Helper.Department);

            _db.LoggingModels.Add(loggingModel);
            _db.Departments.Remove(departmentObj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
        #endregion

        #region Update
        //  GET-Update
        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Departments.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        
        //  POST-Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePost(Department departmentObj)
        {
            var currentUserEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            if (ModelState.IsValid)
            {
                DateTime currentTime = DateTime.Now;
                LoggingModel loggingModel = new LoggingModel(0, currentUserEmail, departmentObj.DepartmentName, currentTime, Helper.Update, Helper.Department);
             
                _db.LoggingModels.Add(loggingModel);
                _db.Departments.Update(departmentObj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(departmentObj);
        }
        #endregion
    }
}