using System.Security.Claims;
using Biblioteczka.Contracts.Request;
using Biblioteczka.Services.Interfaces;

namespace Biblioteczka.API.Endpoints
{
    public static class MovieEndpoints
    {
        public static void MapMovieEndpoints(this WebApplication app)
        {
            app.MapGet("/api/movies", async (IMovieService movieService) => 
            {
                var movies = await movieService.GetMovies();

                return Results.Ok(movies);
            });

            app.MapPost("/api/movies", async (AddUpdateMovieDto addUpdateMovie, IMovieService movieService) =>
            {

                var result = await movieService.AddUpdateMovieDto(addUpdateMovie);

                return result.Match(
                    onSuccess: movie => Results.Created($"/api/movies/{movie.Id}", movie),
                    onFailure: error => Results.BadRequest(new { message = error.Message })
                    );
            }).DisableAntiforgery().RequireAuthorization(policy => policy.RequireRole("Admin"));

            app.MapGet("/api/movies/{id:int}", async (int id, IMovieService movieService) =>
            {
                var movie = await movieService.GetMovie(m => m.Id == id);

                return Results.Ok(movie);
            });

            app.MapPost("/api/movies/poster", async (IFormFile file, ClaimsPrincipal user, IMovieService movieService) =>
            {
                var result = await movieService.AddPosterAsync(file, user);

                return result.Match(
                    onSuccess: url => Results.Created(url, null),
                    onFailure: err => Results.BadRequest(new {message = err.Message})
                    );
            }).DisableAntiforgery().RequireAuthorization(policy => policy.RequireRole("Admin"));
        }
    }
}
