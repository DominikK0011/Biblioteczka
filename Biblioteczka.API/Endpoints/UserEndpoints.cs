using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Contracts.Request;
using Biblioteczka.Contracts.Response;
using Biblioteczka.Services.Interfaces;

namespace Biblioteczka.API.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/user").WithTags("User");

            group.MapPost("/register", async (RegisterUserRequest user, IUserService userService) =>
            {
                var result = await userService.RegisterUserAsync(user);

                return result.Match(
                    onSuccess: _ => Results.Created(),
                    onFailure: error => Results.UnprocessableEntity(new {message = error.Message})
                    );
            });

            group.MapGet("/{id:int}", async (int id, IUserService userService) =>
            {
                var result = await userService.GetUserAsync(u => u.Id == id);

                return result.Match(
                    onSuccess: user => Results.Ok(user),
                    onFailure: error => Results.NotFound(new {message = error.Message})
                    );
            });

            group.MapGet("/", async (IUserService userService) =>
            {
                var result = await userService.GetUsersAsync();

                return result.Match(
                    onSuccess: users => Results.Ok(users),
                    onFailure: error => Results.NotFound(new {message = error.Message})
                    );
            }).DisableAntiforgery().RequireAuthorization(policy => policy.RequireRole("Admin"));

            group.MapPut("/{id:int}", async (AddUpdateUserDto updateUser, IUserService userService) =>
            {
                var result = await userService.AddUpdateUserAsync(updateUser);

                return result.Match(
                    onSuccess: user => Results.Ok(user),
                    onFailure: error => Results.BadRequest(error.Message)
                    );
            }).DisableAntiforgery().RequireAuthorization();

            group.MapPost("/login", async (LoginUserRequest loginData, IUserService userService) =>
            {
                var result = await userService.LoginUserAsync(loginData);

                return result.Match(
                    onSuccess: loginResponse => Results.Ok(loginResponse),
                    onFailure: (error) => Results.BadRequest(new { message = error.Message })
                        );
            });

            group.MapPost("/avatar", async(IFormFile file, ClaimsPrincipal user, IUserService userService) =>
            {
                var response = await userService.SetAvatarToUserAsync(file, user);

                return response.Match(
                    onSuccess: url => Results.Ok(new AvatarUrlResponse(url)),
                    onFailure: err => Results.BadRequest(new { message = err.Message })
                    );
            }).DisableAntiforgery().RequireAuthorization();
        }
    }
}
