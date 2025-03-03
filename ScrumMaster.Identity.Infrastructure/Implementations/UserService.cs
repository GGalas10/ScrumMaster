using Microsoft.AspNetCore.Identity;
using ScrumMaster.Identity.Core.Models;
using ScrumMaster.Identity.Infrastructure.Commands;
using ScrumMaster.Identity.Infrastructure.Contracts;

namespace ScrumMaster.Identity.Infrastructure.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtHandler _jwtHandler;
        public UserService(UserManager<AppUser> userManager,IJwtHandler jwtHandler)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }
        public async Task<string> RegisterUser(RegisterUserCommand command)
        {
            if (command == null)
                throw new Exception("Command_Is_Null");

            if (string.IsNullOrWhiteSpace(command.email))
                throw new Exception("Email_Cannot_Be_Null");

            if (string.IsNullOrWhiteSpace(command.password))
                throw new Exception("Password_Cannot_Be_Null");

            var checkEmail = await _userManager.FindByEmailAsync(command.email);
            if (checkEmail != null)
                throw new Exception("User_Email_Already_Exist");

            var checkName = await _userManager.FindByNameAsync($"{command.firstName} {command.lastName}");
            if (checkName != null)
                throw new Exception("User_Name_Already_Exist");

            var newUser = new AppUser()
            {
                Email = command.email,
                UserName = $"{command.firstName} {command.lastName}",
                FirstName = command.firstName ,
                LastName = command.lastName ,
            };

            var result = await _userManager.CreateAsync(newUser, command.password);
            if (!result.Succeeded)
                throw new Exception("Cannot_Register_User");


            var user = await _userManager.FindByEmailAsync(command.email);
            if (user == null)
                throw new Exception("Something_Wrong");

            var isLoged = await _userManager.CheckPasswordAsync(user, command.password);
            if (!isLoged)
                throw new Exception("Wrong_Credentials");

            return _jwtHandler.CreateToken(user);
        }

        public async Task<string> LoginUser(LoginUserCommand command)
        {
            if (command == null)
                throw new Exception("Command_Is_Null");

            var user = await _userManager.FindByEmailAsync(command.email);
            if (user == null)
                throw new Exception("Something_Wrong");

            var isLoged = await _userManager.CheckPasswordAsync(user, command.password);
            if (isLoged)
                throw new Exception("Wrong_Credentials");

            return _jwtHandler.CreateToken(user);
        }
    }
}
