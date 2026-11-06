using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
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

        public async Task<Result<MovieDto>> AddUpdateMovieDto(AddUpdateMovieDto addUpdateMovieDto)
        {
            var movie = mapper.Map<Movie>(addUpdateMovieDto);
            if (addUpdateMovieDto.Id == null)
            {
                context.Movies.Add(movie);
            }
            else
            {
                context.Movies.Update(movie);
            }
            await context.SaveChangesAsync();

            var movieDto = mapper.Map<MovieDto>(movie);

            return Result<MovieDto>.Success(movieDto);
        }

        public async Task<Result<MovieDto>> GetMovie(Expression<Func<Movie, bool>> filterExpression)
        {
            var movie = await context.Movies.FirstOrDefaultAsync(filterExpression);
            
            if (movie == null)
                return Result<MovieDto>.Failure(new Error.Validation("User not found"));
            
            var movieDto = mapper.Map<MovieDto>(movie);

            return Result<MovieDto>.Success(movieDto);
        }

        public async Task<IEnumerable<MovieDto>> GetMovies(Expression<Func<Movie, bool>>? filterExpression = null)
        {
            var movies = context.Movies.AsQueryable();

            if(filterExpression != null)
                movies = movies.Where(filterExpression);

            var moviesDto = mapper.Map<List<MovieDto>>(movies);

            return moviesDto;
        }
    }
}