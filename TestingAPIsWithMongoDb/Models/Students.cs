using MongoDB.Bson;

namespace TestingAPIsWithMongoDb.Models
{
    public class Students
    {
        public ObjectId Id { get; set; }
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
