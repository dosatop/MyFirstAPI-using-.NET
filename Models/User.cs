using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyFirstApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; } 
        public string Role { get; set; } = "User"; // default role
    }
}