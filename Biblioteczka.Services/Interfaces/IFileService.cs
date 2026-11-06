using System;
using System.Collections.Generic;
using System.Text;
using Biblioteczka.Shared;
using Microsoft.AspNetCore.Http;

namespace Biblioteczka.Services.Interfaces
{
    public interface IFileService
    {
        public Task<Result<string>> SaveFileAsync(IFormFile file, string destinationFolder);
    }
}
