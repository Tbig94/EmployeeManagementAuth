using EmployeeManagementAuth.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementAuth.Controllers
{
    public class LoggingController : Controller
    {
        #region Fields
        private readonly ApplicationDbContext _db;
        #endregion

        #region Ctor
        public LoggingController(ApplicationDbContext db)
        {
            _db = db;
        }
        #endregion

        public IActionResult Index()
        {
            IEnumerable<LoggingModel> objList = _db.LoggingModels;
            return View(objList);
        }
    }
}
