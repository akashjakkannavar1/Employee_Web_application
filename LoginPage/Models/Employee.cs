using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace LoginPage.Models
{
    public class Employee
    {
        [BsonId]
        public double _id { get; set; }
        public string name { get; set; }
        public string designation { get; set; }
        public string departmentName { get; set; }
        public int salary { get; set; }
    }
}
