using System.Linq.Expressions;
using System.Security.Claims;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Contracts.Request;
using Biblioteczka.Contracts.Response;
using Biblioteczka.Model.DataModel;
using Biblioteczka.Shared;
using Microsoft.AspNetCore.Http;

namespace Biblioteczka.Services.Interfaces
{
    public interface IUserService
    {
        //public Task<Result<UserDto>> AddUpdateUserAsync(AddUpdateUserDto addUpdateUser);
        public Task<Result<UserDto>> GetUserAsync(int id);
        public Task<Result<IEnumerable<UserDto>>> GetUsersAsync(Expression<Func<User, bool>>? filterPredicate = null);
       
        public Task<Result<bool>> RegisterUserAsync(RegisterUserRequest userData);
        public Task<Result<LoginResponse>> LoginUserAsync(LoginUserRequest userData);
        public Task<Result<string>> SetAvatarToUserAsync(IFormFile file, ClaimsPrincipal tmp);
        public Task<Result<UserDto>> UpdateUserDataAsync(int id, UpdateUserRequest userData);
        
    }
}
