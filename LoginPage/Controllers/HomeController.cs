using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginPage.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using LoginPage.Services;

namespace LoginPage.Controllers
{
    public class HomeController : Controller
    {
        public IMongoDatabase database;
        private readonly EmployeeSystemService loginPageService;
        public HomeController(EmployeeSystemService loginPageService)
        {
            this.loginPageService = loginPageService;
        }
        public IMongoDatabase GetMongoDatabase()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            return client.GetDatabase("EmployeeLogin");

        }
        public IActionResult AdminLogin()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AdminPage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminPage(string username,string password )
        {
            database = GetMongoDatabase();
            var admin = database.GetCollection<Admin>("Admin").Find(s => s.username == username && s.password==password).FirstOrDefault();
            if(admin==null)
            {
                return RedirectToAction("AdminLogin");
            }
            var emp = loginPageService.GetEmp(username,password);
            var listEmp = loginPageService.GetEmpList(emp);
            return View(listEmp);
        }

        [HttpGet]
        public IActionResult DisplayAllEmp()
        {
            database = GetMongoDatabase();
            var employees = database.GetCollection<Employee>("Employee").Find(s => true).ToList();
            var employees2 = employees.OrderBy(s => s._id).ToList();
            return View(employees2);
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public ActionResult Edit(string name)
        {
            if (name == null)
            {
                return NotFound();
            }
            database = GetMongoDatabase();
            var Employee = database.GetCollection<Employee>("Employee").Find<Employee>(k => k.name == name).FirstOrDefault();
            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
        }

        [HttpGet]
        public ActionResult Delete(string name)
        {
            if (name == null)
            {
                return NotFound();
            }
            database = GetMongoDatabase();
            Employee Employee = database.GetCollection<Employee>("Employee").Find<Employee>(k => k.name == name).First();
            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
        }

        [HttpPost]
        public ActionResult Delete(Employee Employee)
        {
            try
            {
                database = GetMongoDatabase();
                var result = database.GetCollection<Employee>("Employee").DeleteOne<Employee>(k => k.name == Employee.name);
                if (result.IsAcknowledged == false)
                {
                    return BadRequest("Unable to remove Employee " + Employee.name);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Details(string name)
        {
            if (name == null)
            {
                return NotFound();
            }

            database = GetMongoDatabase();

            var Employee = database.GetCollection<Employee>("Employee").Find<Employee>(k => k.name == name).FirstOrDefault();
            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
        }
        [HttpGet]
        public IActionResult AddEmp()
        {
            return View();
        }
        [HttpGet]
        public IActionResult MakeAdmin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MakeAdmin(Admin admin)
        {
            database = GetMongoDatabase();
            database.GetCollection<Admin>("Admin").InsertOne(admin);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddEmp(Employee emp)
        {
            database = GetMongoDatabase();
            database.GetCollection<Employee>("Employee").InsertOne(emp);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(Employee Employee)
        {
            try
            {

                database = GetMongoDatabase();

                var filter = Builders<Employee>.Filter.Eq("_id", Employee._id);

                var updatestatement = Builders<Employee>.Update.Set("_id", Employee._id);
                updatestatement = updatestatement.Set("name", Employee.name);
                updatestatement = updatestatement.Set("designation", Employee.designation);
                updatestatement = updatestatement.Set("departmentName", Employee.departmentName);
                updatestatement = updatestatement.Set("salary", Employee.salary);

                var result = database.GetCollection<Employee>("Employee").UpdateOne(filter, updatestatement);
                if (result.IsAcknowledged == false)
                {
                    return BadRequest("Unable to update Employee  " + Employee.name);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index");
        }

    }
}
