using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Biblioteczka.API.Data;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Contracts.Request;
using Biblioteczka.Model.DataModel;
using Biblioteczka.Services.Interfaces;
using Biblioteczka.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Biblioteczka.Services.ConcreteServices
{
    public class MovieService : BaseService, IMovieService
    {
        private readonly IFileService _fileService;
        public MovieService(IFileService fileService, AppDbContext context, ILogger<MovieService> logger, IMapper mapper) : base(context, logger, mapper)
        {
            _fileService = fileService;
        }

        public async Task<Result<string>> AddPosterAsync(IFormFile file, ClaimsPrincipal adminToken)
        {
            var isAdmin = adminToken.IsInRole(UserRole.Admin.ToString());

            if (!isAdmin)
                return Result<string>.Failure(new Error.Validation("User is not admin"));

            var result = await _fileService.SaveFileAsync(file, "posters");

            return result.Match(
                onSuccess: url => Result<string>.Success(url),
                onFailure: err => Result<string>.Failure(err)
                );
        }

        public async Task<Result<MovieDto>> AddMovieDto(AddMovieRequest addMovieRequest)
        {
            var movie = mapper.Map<Movie>(addMovieRequest);
            var director = await context.People.FirstOrDefaultAsync(d => d.Id == addMovieRequest.DirectorId);

            if (director == null)
                return Result<MovieDto>.Failure(new Error.NotFound(nameof(Person), addMovieRequest.DirectorId));

            movie.Director = director;

            if (addMovieRequest.ActorIds != null)
            {
                var actorIds = addMovieRequest.ActorIds.Distinct().ToList();
                var actors = await context.People.Where(p => actorIds.Contains(p.Id)).ToListAsync();

                if (actorIds.Count != actors.Count)
                {
                    int idNotFoundedActor = actorIds.Except(actors.Select(actor => actor.Id)).First();
                    
                    return Result<MovieDto>.Failure(new Error.NotFound(nameof(Person), idNotFoundedActor));
                }

                movie.Actors = actors;
            }
            await context.Movies.AddAsync(movie);
           
            await context.SaveChangesAsync();

            var movieDto = mapper.Map<MovieDto>(movie);

            return Result<MovieDto>.Success(movieDto);
        }

        public async Task<Result<MovieDto>> GetMovie(int id)
        {
            var movie = await context.Movies
                .Include(m => m.Actors)
                .Include(m => m.Director)
                //.Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (movie == null)
                return Result<MovieDto>.Failure(new Error.NotFound(nameof(Movie).ToString(), id));
            
            var movieDto = mapper.Map<MovieDto>(movie);

            return Result<MovieDto>.Success(movieDto);
        }

        public async Task<IEnumerable<MovieDto>> GetMovies(Expression<Func<Movie, bool>>? filterExpression = null)
        {
            var movies = context.Movies
                .Include(m => m.Actors)
                .Include(m => m.Director)
                //.Include(m => m.Reviews)
                .AsQueryable();

            if(filterExpression != null)
                movies = movies.Where(filterExpression);
            var moviesEntities = await movies.ToListAsync();

            var moviesDto = mapper.Map<List<MovieDto>>(moviesEntities);

            return moviesDto;
        }
    }
}
//TODO: Change ReviewDto to simple contract, then test comment code