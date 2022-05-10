using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq;
using TestingAPIsWithMongoDb.Models;

namespace TestingAPIsWithMongoDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public StudentController (IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]   
        public IActionResult GetStudent()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("StudentAppConn"));

            var dbList = dbClient.GetDatabase("TestingDb").GetCollection<Students>("Students").AsQueryable();

            return new JsonResult(dbList);
        }


        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("StudentAppConn"));

            var filter = Builders<Students>.Filter.Eq("StudentId", id);

           var getById= dbClient.GetDatabase("TestingDb").GetCollection<Students>("Students").Find(filter).ToList();

            return new JsonResult(getById);
        }



        [HttpPost]
        public IActionResult Post(Students students)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("StudentAppConn"));

            int Id = dbClient.GetDatabase("TestingDb").GetCollection<Students>("Students").AsQueryable().Count();
            students.StudentId = Id + 1;

            dbClient.GetDatabase("TestingDb").GetCollection<Students>("Students").InsertOne(students);

            return new JsonResult("Added Successfully");
        }



        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("StudentAppConn"));

            var filter = Builders<Students>.Filter.Eq("StudentId", id);

#pragma warning disable CS0618 // Type or member is obsolete
            var matchedDocument = dbClient.GetDatabase("TestingDb").GetCollection<Students>("Students").Count(filter);
#pragma warning restore CS0618 // Type or member is obsolete

            if (matchedDocument > 0)
            {
                dbClient.GetDatabase("TestingDb").GetCollection<Students>("Students").DeleteOne(filter);

                return new JsonResult("Deleted Successfully");
            }
            return NotFound();
        }




        [HttpPut]
        public IActionResult Put(Students students)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("StudentAppConn"));

            var filter = Builders<Students>.Filter.Eq("StudentId", students.StudentId);

            var update = Builders<Students>.Update.Set("StudentId", students.StudentId)
                                                    .Set("Name", students.Name)
                                                    .Set("Phone", students.Phone);

            dbClient.GetDatabase("TestingDb").GetCollection<Students>("Students").UpdateOne(filter, update);

            return new JsonResult("Updated Successfully");
        }

    }
}
