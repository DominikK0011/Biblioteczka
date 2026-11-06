using System;
using System.Collections.Generic;
using System.Text;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Shared;
using Biblioteczka.UI.Service.Interfaces;
using Microsoft.Win32;

namespace Biblioteczka.UI.Service.ConcreteServices
{
    public class MovieService : IMovieService
    {
        private readonly IApiClient _apiClient;

        public MovieService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<MovieDto>> GetMoviesAsync()
        {
            var response = await _apiClient.GetAsync<List<MovieDto>>("/api/movies");
            if (response.IsSuccess)
            {
                foreach(var m in response.Value)
                {
                    m.PosterUrl = _apiClient.GetFileUrl(m.PosterUrl);
                }
                return response.Value;
            }

            return new List<MovieDto> { new MovieDto { Title = "Idk" } };
        }

        public async Task<Result<string>> UploadPosterAsync(string filePath)
        {
            var result = await _apiClient.UploadFileAsync("api/movies/poster", filePath);

            return result.Match(
                onSuccess: url => Result<string>.Success(url),
                onFailure: err => Result<string>.Failure(err)
                );
        }
    }
}
