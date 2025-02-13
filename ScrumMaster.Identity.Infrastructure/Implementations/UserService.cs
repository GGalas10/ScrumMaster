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

            var newUser = new AppUser()
            {
                Email = command.email,
                UserName = $"{command.firstName} {command.lastName}",
                FirstName = command.firstName ,
                LastName = command.lastName ,
            };
            var result = await _userManager.CreateAsync(newUser, command.password);
            if (!result.Succeeded)
                throw new Exception("Cannot register User");


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
