using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using LoginPage.Models;

namespace LoginPage.Services
{
    
    public class EmployeeSystemService
    {
        public IMongoDatabase database;
        public IMongoDatabase GetMongoDatabase()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            return client.GetDatabase("EmployeeLogin");

        }

        public Employee GetEmp(string username,string password)
        {
            database = GetMongoDatabase();
            var admin = database.GetCollection<Admin>("Admin").Find(s => s.username == username).FirstOrDefault();
            var emp = database.GetCollection<Employee>("Employee").Find(s => s._id == admin.eid).FirstOrDefault();
            return emp;
        }

        public List<Employee> GetEmpList(Employee emp)
        {
            var emplist = database.GetCollection<Employee>("Employee").Find(s => s.departmentName == emp.departmentName).ToList();
            return emplist;
        }


    }
}
