using Microsoft.AspNetCore.Identity;
using Moq;
using ScrumMaster.Identity.Core.Models;

namespace ScrumMaster.Identity.Tests.Common
{
    public class MockUserManager
    {
        public static Mock<UserManager<AppUser>> GetUserManager(List<AppUser> users)
        {
            var store = new Mock<IUserStore<AppUser>>();
            var passwordHasher = new Mock<IPasswordHasher<AppUser>>();
            IList<IUserValidator<AppUser>> userValidators = new List<IUserValidator<AppUser>>
            {
                new UserValidator<AppUser>()
            };
            
            IList<IPasswordValidator<AppUser>> passwordValidators = new List<IPasswordValidator<AppUser>>
            {
                new PasswordValidator<AppUser>()
            };
            userValidators.Add(new UserValidator<AppUser>());
            passwordValidators.Add(new PasswordValidator<AppUser>());
            var userManager = new Mock<UserManager<AppUser>>(store.Object, null, passwordHasher.Object, userValidators, passwordValidators,null, null, null, null);
            userManager.Setup(x => x.DeleteAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            userManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<AppUser, string>((x, y) => users.Add(x));
            userManager.Setup(x => x.UpdateAsync(It.IsAny<AppUser>())).ReturnsAsync(IdentityResult.Success);
            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((string value) => users.FirstOrDefault(x=>x.Email == value));
            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((string value) => users.FirstOrDefault(x=>x.UserName == value));
            userManager.Setup(x => x.CheckPasswordAsync(It.IsAny<AppUser>(),It.IsAny<string>())).ReturnsAsync((AppUser user,string pass) => users.Any(x=>x.UserName == user.UserName));
            return userManager;
        }
        public static List<AppUser> GetUsersList()
        {
            List<AppUser> usersList = new List<AppUser>();
            return usersList;
        }
    }
}
