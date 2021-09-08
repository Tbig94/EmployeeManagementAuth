using EmployeeManagementAuth.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DepartmentController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Department> objList = _db.Departments;
            return View(objList);
        }

        //  GET-Create
        public IActionResult Create()
        {
            return View();
        }

        //  POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(Department obj)
        {
            if (ModelState.IsValid)
            {
                _db.Departments.Add(obj);
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
            var obj = _db.Departments.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Departments.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

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
        public IActionResult UpdatePost(Department obj)
        {
            if (ModelState.IsValid)
            {
                _db.Departments.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }

    }
}
