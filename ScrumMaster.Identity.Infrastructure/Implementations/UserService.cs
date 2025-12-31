using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScrumMaster.Identity.Core.Models;
using ScrumMaster.Identity.Infrastructure.Commands;
using ScrumMaster.Identity.Infrastructure.Contracts;
using ScrumMaster.Identity.Infrastructure.DTO;
using ScrumMaster.Identity.Infrastructure.Exceptions;

namespace ScrumMaster.Identity.Infrastructure.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtHandler _jwtHandler;
        private readonly IRefreshTokenService _refreshTokenService;
        public UserService(UserManager<AppUser> userManager,IJwtHandler jwtHandler,IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<AuthDTO> RegisterUser(RegisterUserCommand command)
        {
            if (command == null)
                throw new BadRequestException("Command_Is_Null");

            if (string.IsNullOrWhiteSpace(command.email))
                throw new BadRequestException("Email_Cannot_Be_Null");

            if (string.IsNullOrWhiteSpace(command.password))
                throw new BadRequestException("Password_Cannot_Be_Null");

            if (string.IsNullOrWhiteSpace(command.firstName))
                throw new BadRequestException("FirstName_Cannot_Be_Null");

            if (string.IsNullOrWhiteSpace(command.lastName))
                throw new BadRequestException("LastName_Cannot_Be_Null");

            if (string.IsNullOrWhiteSpace(command.userName))
                throw new BadRequestException("UserName_Cannot_Be_Null");

            if (command.password.Length < 10)
                throw new BadRequestException("Password_Is_Too_Short");

            if (command.password != command.confirmPassword)
                throw new BadRequestException("Passwords_Incorrect");

            var checkEmail = await _userManager.FindByEmailAsync(command.email);
            if (checkEmail != null)
                throw new BadRequestException("User_Email_Already_Exist");

            var checkName = await _userManager.FindByNameAsync(command.userName);
            if (checkName != null)
                throw new BadRequestException("User_Name_Already_Exist");

            var newUser = new AppUser()
            {
                Email = command.email,
                UserName = command.userName,
                FirstName = command.firstName ,
                LastName = command.lastName ,
                RegisterAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
            };

            var result = await _userManager.CreateAsync(newUser, command.password);
            if (!result.Succeeded)
            {
                var errors = "";
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Code}\n";
                }
                throw new Exception(errors);
            }
            var user = await _userManager.FindByEmailAsync(command.email);
            if (user == null)
                throw new Exception("Something_Wrong");

            var isLoged = await _userManager.CheckPasswordAsync(user, command.password);
            if (!isLoged)
                throw new Exception("Wrong_Credentials");

            var jwtToken = _jwtHandler.CreateToken(user);
            var refreshToken = await _refreshTokenService.CreateRefreshToken(Guid.Parse(user.Id));
            return new AuthDTO()
            {
                jwtToken = jwtToken,
                refreshToken = refreshToken,
                userName = user.UserName
            };
        }
        public async Task<AuthDTO> LoginUser(LoginUserCommand command)
        {
            if (command == null)
                throw new BadRequestException("Command_Is_Null");

            var user = await _userManager.FindByEmailAsync(command.email);
            if (user == null)
                throw new BadRequestException("Wrong_Credentials");

            var isLoged = await _userManager.CheckPasswordAsync(user, command.password);
            if (!isLoged)
                throw new BadRequestException("Wrong_Credentials");

            var jwtToken = _jwtHandler.CreateToken(user);
            var refreshToken = await _refreshTokenService.CreateRefreshToken(Guid.Parse(user.Id));

            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return new AuthDTO()
            {
                jwtToken = jwtToken,
                refreshToken = refreshToken,
                userName = user.UserName
            };
        }
        public async Task<string> GetUserInfo(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return null;
            return user.UserName;
        }
        public async Task<List<UserDTO>> GetUsers(List<Guid> userIds)
        {
            var userIdsAsString = userIds.Select(x => x.ToString()).ToList();
            var users = await _userManager.Users.Where(u => userIdsAsString.Contains(u.Id)).ToListAsync();
            return users.Select(x => 
            {
                return new UserDTO()
                {
                    id = Guid.Parse(x.Id),
                    firstName = x.FirstName,
                    lastName = x.LastName
                };
            }
            ).ToList();
        }
        public async Task<UserDTO> GetUserById(Guid userId)
        {
            if(userId == Guid.Empty)
                throw new Exception("UserId_Is_Empty");
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception("User_Not_Found");
            return new UserDTO()
            {
                id = Guid.Parse(user.Id),
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                registerAt = user.RegisterAt,
                lastLoginAt = user.LastLoginAt
            };
        }
        public async Task<List<UserListDTO>> FindUsers(string filter,Guid userId)
        {
            var users = await _userManager.Users.Where(x => (x.FirstName.Contains(filter) || x.LastName.Contains(filter) || x.Email.Contains(filter)) && x.Id != userId.ToString()).ToListAsync();
            return users.Select(x =>
            {
                return new UserListDTO()
                {
                    id = Guid.Parse(x.Id),
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    email = x.Email,
                };
            }).ToList();
        }
    }
}
