using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Services;

namespace MyFirstApi.Endpoints
{
    public static class UserEndpoints
    {
        // private static readonly CurrentUserService currentUserService;

        public static void MapUserEndpoints(this WebApplication app)
        {
                    
            app.MapGet("/users", ([FromServices] MongoServices service) =>
            {
                return service.GetAllUsers();
            }).RequireAuthorization(policy => policy.RequireRole("Admin")); // Only Admins can access this endpoint

            // app.MapPost("/users", ([FromServices] MongoServices service, [FromBody] User newUser) =>
            // {
            //     service.CreateUser(newUser);
            //     return Results.Ok(newUser);
            // });

            app.MapGet("/users/{id}", ([FromServices] MongoServices service,[FromRoute] string id) =>
            {
                var user = service.GetUserById(id);

                if (user == null)
                {
                    return Results.NotFound("User not found");
                }

                return Results.Ok(user);
            }).RequireAuthorization();

            app.MapPut("/users/{id}", ([FromRoute] string id, [FromBody] UpdateUserDto user, [FromServices] MongoServices service) =>
            {
                return service.UpdateUser(id, user.Name, user.Email);
            }).RequireAuthorization();


            app.MapDelete("/users/{id}", ([FromServices] MongoServices service, [FromRoute] string id) =>
            {
                var user = service.GetUserById(id);

                if (user == null)
                {
                    return Results.NotFound("User not found");
                }

                service.DeleteUser(user);

                return Results.Ok("User deleted successfully");
            }).RequireAuthorization();   


             app.MapPut("/users/admin", ([FromServices] AdminService service,  [FromServices] CurrentUserService currentUserService, [FromBody] MakeAdminDto patchDoc) =>
             {
                var id = currentUserService.UserId!;
    
                      if (patchDoc == null)
        return Results.BadRequest("Request body is required");

                
                 service.SetUserRole(id, patchDoc.role == "Admin" ? "Admin" : "User");
           
                 return Results.Ok("User role updated successfully");
             }).RequireAuthorization(); 
          }

          
    }
}