using System.Security.Claims;
using Biblioteczka.Contracts.Request;
using Biblioteczka.Contracts.Response;
using Biblioteczka.Services.Interfaces;
using Biblioteczka.Shared;

namespace Biblioteczka.API.Endpoints
{
    public static class MovieEndpoints
    {
        public static void MapMovieEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/movies").WithTags("Movie");

            group.MapGet("/", async (IMovieService movieService) => 
            {
                var movies = await movieService.GetMovies();

                return Results.Ok(movies);
            });

            group.MapPost("/", async (AddMovieRequest addMovieRequest, IMovieService movieService) =>
            {

                var result = await movieService.AddMovieDto(addMovieRequest);

                return result.Match(
                    onSuccess: movieDto => Results.Created($"/api/movies/{movieDto.Id}", movieDto),
                    onFailure: error => 
                        {
                            if (error is Error.NotFound)
                                return Results.NotFound(new { message = error.Message });
                            else if (error is Error.Validation)
                                return Results.BadRequest(new { message = error.Message });
                            else
                                return Results.BadRequest();
                        }
                    );
            }).DisableAntiforgery().RequireAuthorization(policy => policy.RequireRole("Admin"));

            group.MapGet("/{id:int}", async (int id, IMovieService movieService) =>
            {
                var result = await movieService.GetMovie(m => m.Id == id);

                return result.Match(
                    onSuccess: movie => Results.Ok(movie),
                    onFailure: error =>
                    {
                        if (error is Error.NotFound)
                            return Results.NotFound(new { message = error.Message });
                        else if (error is Error.Validation)
                            return Results.BadRequest(new { message = error.Message });
                        else
                            return Results.BadRequest();
                    }
                    );
            });

            group.MapPost("/poster", async (IFormFile file, ClaimsPrincipal user, IMovieService movieService) =>
            {
                var result = await movieService.AddPosterAsync(file, user);

                return result.Match(
                    onSuccess: url => Results.Ok(new AvatarUrlResponse(url)),
                    onFailure: err => Results.BadRequest(new {message = err.Message})
                    );
            }).DisableAntiforgery().RequireAuthorization(policy => policy.RequireRole("Admin"));
        }
    }
}
