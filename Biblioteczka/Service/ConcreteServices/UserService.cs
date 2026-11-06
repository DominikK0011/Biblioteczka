using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Contracts.Request;
using Biblioteczka.Contracts.Response;
using Biblioteczka.Shared;
using Biblioteczka.UI.Service.Interfaces;
using Biblioteczka.UI.Session;

namespace Biblioteczka.UI.Service.ConcreteServices
{
    public class UserService : IUserService
    {
        private readonly IApiClient _apiGateway;

        public UserService(IApiClient apiGateway)
        {
            _apiGateway = apiGateway;
        }

        public async Task<Result<string>> ChangeAvatarAsync(string avatarPath)
        {
            var response = await _apiGateway.UploadFileAsync("api/user/avatar", avatarPath);

            return response.Match(
                onSuccess: url => Result<string>.Success(url),
                onFailure: err => Result<string>.Failure(err)
                );
        }

        public async Task<Result<bool>> LoginUserAsync(LoginUserRequest userLoginDto)
        {
            var response = await _apiGateway.PostAsync<LoginResponse>("/api/user/login", userLoginDto);
            return response.Match(
                onSuccess: loginResponse => {
                    loginResponse.User.AvatarUrl = _apiGateway.GetFileUrl(loginResponse.User.AvatarUrl);
                    UserSession.Login(loginResponse.Token, loginResponse.User);
                    _apiGateway.AssignTokenToHttpClient(loginResponse.Token);
                    return Result<bool>.Success(true);
                },
                onFailure: err => { return Result<bool>.Failure(err); }
                );
        }

        public async Task<Result<bool>> RegisterUserAsync(RegisterUserRequest userdto)
        {
            var response = await _apiGateway.PostAsync("/api/user/register", userdto);
            
            return response.Match(
                onSuccess: userData =>
                {
                    return Result<bool>.Success(true);
                },
                onFailure: error => 
                {
                    return Result<bool>.Failure(error); 
                }
                );
        }
    }
}
