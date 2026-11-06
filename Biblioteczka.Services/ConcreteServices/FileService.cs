using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Biblioteczka.API.Data;
using Biblioteczka.Services.Interfaces;
using Biblioteczka.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Biblioteczka.Services.ConcreteServices
{
    public class FileService : BaseService, IFileService
    {
        private readonly IWebHostEnvironment _environment;
        public FileService(IWebHostEnvironment environment, AppDbContext context, ILogger<FileService> logger, IMapper mapper) : base(context, logger, mapper)
        {
            _environment = environment;
        }

        public async Task<Result<string>> SaveFileAsync(IFormFile file, string destinationFolder)
        {
            if (file.Length == 0)
                return Result<string>.Failure(new Error.Validation("File is empty"));

            var path = Path.Combine(_environment.WebRootPath, "uploads", destinationFolder);

            Directory.CreateDirectory(path);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(path, fileName);

            await using var stream = File.Create(filePath);
            await file.CopyToAsync(stream);

            var url = $"/uploads/{destinationFolder}/{fileName}";

            return Result<string>.Success(url);
        }
    }
}
