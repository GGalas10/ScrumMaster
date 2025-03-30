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

            if (string.IsNullOrWhiteSpace(command.firstName))
                throw new Exception("FirstName_Cannot_Be_Null");

            if (string.IsNullOrWhiteSpace(command.lastName))
                throw new Exception("LastName_Cannot_Be_Null");

            if (string.IsNullOrWhiteSpace(command.userName))
                throw new Exception("UserName_Cannot_Be_Null");

            if (command.password.Length < 6)
                throw new Exception("Password_Is_Too_Short");

            if (command.password != command.confirmPassword)
                throw new Exception("Passwords_Incorrect");

            var checkEmail = await _userManager.FindByEmailAsync(command.email);
            if (checkEmail != null)
                throw new Exception("User_Email_Already_Exist");

            var checkName = await _userManager.FindByNameAsync(command.userName);
            if (checkName != null)
                throw new Exception("User_Name_Already_Exist");

            var newUser = new AppUser()
            {
                Email = command.email,
                UserName = command.userName,
                FirstName = command.firstName ,
                LastName = command.lastName ,
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

            return _jwtHandler.CreateToken(user);
        }

        public async Task<string> LoginUser(LoginUserCommand command)
        {
            if (command == null)
                throw new Exception("Command_Is_Null");

            var user = await _userManager.FindByEmailAsync(command.email);
            if (user == null)
                throw new Exception("Wrong_Credentials");

            var isLoged = await _userManager.CheckPasswordAsync(user, command.password);
            if (!isLoged)
                throw new Exception("Wrong_Credentials");

            return _jwtHandler.CreateToken(user);
        }
    }
}
