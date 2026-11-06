using System;
using System.Collections.Generic;
using System.Text;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Shared;

namespace Biblioteczka.UI.Service.Interfaces
{
    public interface IMovieService
    {
        public Task<List<MovieDto>> GetMoviesAsync();
        public Task<Result<string>> UploadPosterAsync(string filePath);
    }
}
