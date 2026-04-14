namespace MyFirstApi.Services
{
    public class AdminService
    {
        private readonly MongoServices _mongoService;
        public AdminService(MongoServices mongoService)
        {   
            _mongoService = mongoService;
        }
        public void SetUserRole(string userId, string role)

        {
            // Here you would typically interact with your database to update the user's role
            // For example:
            var user = _mongoService.GetUserById(userId);
            if (user != null) Results.NotFound("User not found");

            user!.Role = role;
            _mongoService.UpdateUserRole(userId, role);

            // You might also want to add some logging or additional checks here
            // ...
           
        }
    }
}