using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Biblioteczka.Shared;

namespace Biblioteczka.UI.Service.Interfaces
{
    public interface IApiClient
    {
        public Task<Result<T?>> PostAsync<T>(string endpoint, object data);
        public Task<Result<T?>> GetAsync<T>(string endpoint);
        public Task<Result<bool>> PostAsync(string endpoint, object data);
        public Task<Result<string>> UploadFileAsync(string endpoint, string filePath);
        public Task<Result<T?>> PutAsync<T>(string endpoint, object data);
        public void AssignTokenToHttpClient(string token);
        public string GetServerAddress();
        public string GetFileUrl(string path);
    }
}
