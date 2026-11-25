using System.Security.Claims;
using Biblioteczka.Contracts.Request;
using Biblioteczka.Contracts.Response;
using Biblioteczka.Services.Interfaces;
using Biblioteczka.Shared;

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
                    onFailure: error =>
                    {
                        if (error is Error.Conflict)
                            return Results.Conflict(new { message = error.Message });
                        return Results.BadRequest();
                    }
                    );
            });

            group.MapGet("/{id:int}", async (int id, IUserService userService) =>
            {
                var result = await userService.GetUserAsync(id);

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

            group.MapPut("/{id:int}", async (int id, UpdateUserRequest updateUser, IUserService userService) =>
            {
                var result = await userService.UpdateUserDataAsync(id, updateUser);

                return result.Match(
                    onSuccess: user => Results.Ok(user),
                    onFailure: error =>
                    {
                        if (error is Error.Conflict)
                            return Results.Conflict(new { message = error.Message });
                        else if (error is Error.NotFound)
                            return Results.NotFound(new { message = error.Message });
                        return Results.BadRequest();
                    }
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
                    onFailure: error =>
                    {
                        if (error is Error.Conflict)
                            return Results.Conflict(new { message = error.Message });
                        else if (error is Error.NotFound)
                            return Results.NotFound(new { message = error.Message });
                        return Results.BadRequest();
                    }
                    );
            }).DisableAntiforgery().RequireAuthorization();
        }
    }
}
