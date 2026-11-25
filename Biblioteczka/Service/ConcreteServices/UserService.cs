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
                onSuccess: url => {
                    string fullUrl = _apiGateway.GetFileUrl(url);
                    return Result<string>.Success(fullUrl);
                    },
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

        public async Task<Result<UserDto>> UpdateUserAsync(UpdateUserRequest updateUserRequest)
        {
            if (!UserSession.IsLogged || UserSession.ActualUser == null)
                return Result<UserDto>.Failure(new Error.Conflict("User is not logged"));

            var response = await _apiGateway.PutAsync<UserDto>($"/api/user/{UserSession.ActualUser.Id}", updateUserRequest);

            return response.Match(
                onSuccess: userDto =>
                {
                    if(userDto == null)
                        return Result<UserDto>.Failure(new Error.Conflict("User is null"));

                    userDto.AvatarUrl = _apiGateway.GetFileUrl(userDto.AvatarUrl);
                    
                    UserSession.UpdateUser(userDto);

                    return Result<UserDto>.Success(userDto);
                },
                onFailure: error =>
                {
                    return Result<UserDto>.Failure(error);
                }
                );
        }
    }
}
