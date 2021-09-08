using EmployeeManagementAuth.Models;
using EmployeeManagementAuth.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }

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
        public IActionResult CreatePost(EmployeeVM obj)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Add(obj.Employee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }

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
            var obj = _db.Employees.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Employees.Remove(obj);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }


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
        public IActionResult UpdatePost(EmployeeVM obj)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Update(obj.Employee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(obj);
        }


    }
}
