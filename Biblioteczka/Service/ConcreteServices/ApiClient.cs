using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Contracts.Request;
using Biblioteczka.Contracts.Response;
using Biblioteczka.Shared;
using Biblioteczka.UI.Service.Interfaces;
using Biblioteczka.UI.Session;

namespace Biblioteczka.UI.Service.ConcreteServices
{
    public class ApiClient: IApiClient
    {
        private readonly string ServerAddress = "https://localhost:7262";
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(ServerAddress)
            };
        }

        public async Task<Result<T?>> PostAsync<T>(string endpoint, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                var errorMessage = JsonSerializer.Deserialize<ErrorResponse>(errorJson);
                return Result<T?>.Failure(new Error.Validation(errorMessage?.Message ?? "Unknow Error"));
            }
            var resultData = await response.Content.ReadFromJsonAsync<T>();

            return Result<T?>.Success(resultData);
        }

        public async Task<Result<T?>> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                return Result<T?>.Failure(new Error.Validation(errorJson));
            }
            var resultData = await response.Content.ReadFromJsonAsync<T>();
            return Result<T?>.Success(resultData);
        }

        public void AssignTokenToHttpClient(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public string GetServerAddress() => ServerAddress;
        public string GetFileUrl(string path) => new Uri(_httpClient.BaseAddress!, path).ToString();

        public async Task<Result<bool>> PostAsync(string endpoint, object data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                var errorMessage = JsonSerializer.Deserialize<ErrorResponse>(errorJson);
                return Result<bool>.Failure(new Error.Validation(errorMessage?.Message ?? ""));
            }

            return Result<bool>.Success(true);
        }

        public async Task<Result<string>> UploadFileAsync(string endpoint, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return Result<string>.Failure(new Error.Validation("File path is null or empty"));

            var s=
        $"Authorization set: " +
        $"{_httpClient.DefaultRequestHeaders.Authorization != null}";
            var ss = $"Request URL: {_httpClient.BaseAddress}{endpoint}";

            using var form = new MultipartFormDataContent();
            using var fileStream = File.OpenRead(filePath);
            using var fileContent = new StreamContent(fileStream);

            form.Add(fileContent, "file", Path.GetFileName(filePath));

            var response = await _httpClient.PostAsync(endpoint, form);

            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                return Result<string>.Failure(new Error.Validation(errorJson));
            }

            var result = await response.Content.ReadFromJsonAsync<AvatarUrlResponse>();

            if(result is null)
            {
                return Result<string>.Failure(new Error.Validation("Empty response"));
            }

            return Result<string>.Success(result.url);
        }
    }
}
