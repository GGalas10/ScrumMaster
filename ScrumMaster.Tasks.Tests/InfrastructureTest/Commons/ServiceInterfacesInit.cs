using Moq;
using ScrumMaster.Tasks.Infrastructure.Contracts;

namespace ScrumMaster.Tasks.Tests.InfrastructureTest.Commons
{
    public static class ServiceInterfacesInit
    {
        public static IProjectAPIService CreateProjectAPIService()
        {
            var mock = new Mock<IProjectAPIService>();
            mock.Setup(service => service.UserHavePremissions(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<Core.Enums.UserPremissionsEnum>()))
                .ReturnsAsync(true);
            return mock.Object;
        }
        public static ISprintAPIService CreateSprintAPIService()
        {
            var mock = new Mock<ISprintAPIService>();
            mock.Setup(service => service.GetProjectIdBySprintId(
                It.IsAny<Guid>()))
                .ReturnsAsync(Guid.NewGuid());
            mock.Setup(service => service.CheckSprintExist(
                It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            return mock.Object;
            throw new NotImplementedException();
        }
        public static IUserAPIService CreateUserAPIService()
        {
            var mock = new Mock<IUserAPIService>();
            mock.Setup(service => service.GetUserById(
                It.IsAny<Guid>()))
                .ReturnsAsync(new Infrastructure.DTOs.Users.UserDTO
                {
                    id = Guid.NewGuid(),
                    firstName = "John",
                    lastName = "Doe",
                    email = "test@test.pl",
                    registerAt = DateTime.UtcNow,
                    lastLoginAt = DateTime.UtcNow
                });
            return mock.Object;
            throw new NotImplementedException();
        }
    }
}
