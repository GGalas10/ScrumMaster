using Microsoft.AspNetCore.Identity;
using Moq;
using ScrumMaster.Identity.Core.Models;

namespace ScrumMaster.Identity.Tests.Common
{
    public class MockUserManager
    {
        public static Mock<UserManager<TUser>> GetUserManager<TUser>(List<TUser> users)
        where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var passwordHasher = new Mock<IPasswordHasher<TUser>>();
            IList<IUserValidator<TUser>> userValidators = new List<IUserValidator<TUser>>
            {
                new UserValidator<TUser>()
            };
            
            IList<IPasswordValidator<TUser>> passwordValidators = new List<IPasswordValidator<TUser>>
            {
                new PasswordValidator<TUser>()
            };
            userValidators.Add(new UserValidator<TUser>());
            passwordValidators.Add(new PasswordValidator<TUser>());
            var userManager = new Mock<UserManager<TUser>>(store.Object, null, passwordHasher.Object, userValidators, passwordValidators,null, null, null, null);
            userManager.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            userManager.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => users.Add(x));
            userManager.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            return userManager;
        }
        public static List<AppUser> GetUsersList()
        {
            List<AppUser> usersList = new List<AppUser>();
            return usersList;
        }
    }
}
