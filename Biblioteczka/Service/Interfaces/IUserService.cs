using System;
using System.Collections.Generic;
using System.Text;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Contracts.Request;
using Biblioteczka.Shared;

namespace Biblioteczka.UI.Service.Interfaces
{
    public interface IUserService
    {
        public Task<Result<bool>> RegisterUserAsync(RegisterUserRequest userdto);
        public Task<Result<bool>> LoginUserAsync(LoginUserRequest userLoginDto);
        public Task<Result<string>> ChangeAvatarAsync(string avatarPath);
    }
}
