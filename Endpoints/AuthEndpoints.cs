using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Models;
using MyFirstApi.Services;
using BCrypt.Net;

namespace MyFirstApi.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/auth/login", (LoginDto dto, MongoServices service, AuthService authService) =>
            {
                var user = service.GetUserByEmail(dto.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                {
                    return Results.Unauthorized();
                }

                var token = authService.CreateToken(user);

                return Results.Ok(new { Id = user.Id, Name = user.Name, Email = user.Email, AccessToken = token });
            });

            app.MapPost("/auth/register", (RegisterDto dto, MongoServices service) =>
            {
                var user = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Role = "User" // default
                };

                service.CreateUser(user);

                return Results.Ok(new { Message = "User registered successfully", User = user });
            });

        }}}