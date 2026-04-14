using MongoDB.Driver;
using MyFirstApi.Models;

namespace MyFirstApi.Services
{
    public class MongoServices
    {
        private readonly IMongoCollection<User> _usersCollection;

        public MongoServices(IConfiguration config)
      {
            // 👇 THIS is where the "link" is used
            var connectionString = config["MongoDB:ConnectionString"];
            var databaseName = config["MongoDB:Database"];

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            _usersCollection = database.GetCollection<User>("Users");
        }

        public List<User> GetAllUsers() =>
            _usersCollection.Find(user => true).ToList();

        public User GetUserById(string id) =>
            _usersCollection.Find<User>(user => user.Id == id).FirstOrDefault();
        public User GetUserByEmail(string email) =>
            _usersCollection.Find<User>(user => user.Email == email).FirstOrDefault();

        public User CreateUser(User user)
        {
            _usersCollection.InsertOne(user);
            return user;
        }

        public User UpdateUser(string id, string? name, string? email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var update = Builders<User>.Update
                .Set(u => u.Name, name)
                .Set(u => u.Email, email);

            _usersCollection.UpdateOne(filter, update);
            return GetUserById(id);
        }
        public User DeleteUser(User user)
        {
            _usersCollection.DeleteOne(u => u.Id == user.Id);
            return user;
        }
        public User UpdateUserRole(string userId, string role)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Set(u => u.Role, role);

            _usersCollection.UpdateOne(filter, update);
            return GetUserById(userId);
        }
            }
}