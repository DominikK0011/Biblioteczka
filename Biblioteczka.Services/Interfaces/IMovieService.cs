using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Contracts.Request;
using Biblioteczka.Model.DataModel;
using Biblioteczka.Shared;
using Microsoft.AspNetCore.Http;

namespace Biblioteczka.Services.Interfaces
{
    public interface IMovieService
    {
        public Task<Result<MovieDto>> AddUpdateMovieDto(AddUpdateMovieDto addUpdateMovieDto);
        public Task<Result<MovieDto>> GetMovie(Expression<Func<Movie, bool>> filterExpression);
        public Task<IEnumerable<MovieDto>> GetMovies(Expression<Func<Movie, bool>>? filterExpression = null);
        public Task<Result<string>> AddPosterAsync(IFormFile file, ClaimsPrincipal adminToken);
    }
}
