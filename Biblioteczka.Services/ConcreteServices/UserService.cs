using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Biblioteczka.API.Data;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Contracts.Request;
using Biblioteczka.Contracts.Response;
using Biblioteczka.Model.DataModel;
using Biblioteczka.Services.Interfaces;
using Biblioteczka.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Biblioteczka.Services.ConcreteServices
{
    public class UserService : BaseService, IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly IFileService _fileService;
        public UserService(AppDbContext context, ILogger<UserService> logger, IMapper mapper, ITokenService tokenService, IFileService fileService) : base(context, logger, mapper)
        {
            _tokenService = tokenService;
            _fileService = fileService;
        }

        public async Task<Result<UserDto>> AddUpdateUserAsync(AddUpdateUserDto addUpdateUser)
        {
            var user = mapper.Map<User>(addUpdateUser);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(addUpdateUser.Password);

            if (addUpdateUser.Id.HasValue)
            {
                context.Users.Update(user);
            }
            else
            {
                if (context.Users.Any(u => u.Email == addUpdateUser.Email))
                    return Result<UserDto>.Failure(new Error.Validation("Email already exist"));

                if (context.Users.Any(u => u.Username == addUpdateUser.Username))
                    return Result<UserDto>.Failure(new Error.Validation("Username already exist"));

                context.Users.Add(user);
            }

            await context.SaveChangesAsync();
            var userDto = mapper.Map<UserDto>(user);

            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result<UserDto>> GetUserAsync(Expression<Func<User, bool>> filterPredicate)
        {
            var user = await context.Users.FirstOrDefaultAsync(filterPredicate);

            if (user == null)
            {
                return Result<UserDto>.Failure(new Error.Validation($"Not found user with predicate {filterPredicate.ToString()}"));
            }

            var userDto = mapper.Map<UserDto>(user);

            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result<IEnumerable<UserDto>>> GetUsersAsync(Expression<Func<User, bool>>? filterPredicate = null)
        {
            var users = context.Users.AsQueryable();

            if (filterPredicate != null)
                users = users.Where(filterPredicate);

            var usersList = await users.ToListAsync();
            var usersDto = mapper.Map<IEnumerable<UserDto>>(usersList);

            return Result<IEnumerable<UserDto>>.Success(usersDto);
        }

        public async Task<Result<string>> SetAvatarToUserAsync(IFormFile file, ClaimsPrincipal userToken)
        {
            var userIdClaim = userToken.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Result<string>.Failure(new Error.Validation("Not founded user ID"));

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return Result<string>.Failure(new Error.Validation("Invalid user ID"));

            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return Result<string>.Failure(new Error.NotFound(nameof(User), userId));

            var response = await _fileService.SaveFileAsync(file, "avatars");

            if (!response.IsSuccess)
                return Result<string>.Failure(response.Error);

            var avatarUrl = response.Value;

            user.AvatarUrl = avatarUrl;

            await context.SaveChangesAsync();

            return Result<string>.Success(avatarUrl);
        }

        public async Task<Result<bool>> RegisterUserAsync(RegisterUserRequest userData)
        {
            if (context.Users.Any(u => u.Email == userData.Email))
                return Result<bool>.Failure(new Error.Validation("Email already exists"));

            if (context.Users.Any(u => u.Username == userData.Username))
                return Result<bool>.Failure(new Error.Validation("Username already exists"));

            var user = mapper.Map<User>(userData);

            user.AvatarUrl = "uploads/avatars/default.jpg";
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userData.Password);

            context.Users.Add(user);

            await context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<UserDto>> UpdateUserDataAsync(int id,UpdateUserRequest userData)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return Result<UserDto>.Failure(new Error.NotFound(nameof(User), id));

            user.Username = userData.Username;
            user.Email = userData.Email;

            if (!string.IsNullOrWhiteSpace(userData.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userData.Password);

            await context.SaveChangesAsync();

            var userDto = mapper.Map<UserDto>(user);

            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result<LoginResponse>> LoginUserAsync(LoginUserRequest userData)
        {
            if (!userData.Login.Contains('@'))
                return Result<LoginResponse>.Failure(new Error.Validation("Invalid email"));

            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == userData.Login);

            if (user == null)
                return Result<LoginResponse>.Failure(new Error.Validation("User ID not found"));

            string password = userData.Password;
            if (!BCrypt.Net.BCrypt.Verify(userData.Password, user.PasswordHash))
                return Result<LoginResponse>.Failure(new Error.Validation("Wrong password"));

            string token = _tokenService.GenerateToken(user);
            var userDto = mapper.Map<UserDto>(user);

            var loginResponse = new LoginResponse(token, userDto);

            return Result<LoginResponse>.Success(loginResponse);
        }
    }
}
