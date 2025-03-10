using ScrumMaster.Tasks.Infrastructure.Implementations;
using ScrumMaster.Tasks.Tests.InfrastructureTest.Commons;

namespace ScrumMaster.Tasks.Tests.InfrastructureTest
{
    public class TaskServiceTests
    {
        [Fact]
        public async Task CreateTask_WhenCommandIsNull_Should_ThrowException()
        {
            //Arrange
            var dbContext = DbContextInitializer.InitDatabase();
            var service = new TaskService(dbContext);

            //Act
            try
            {
                await service.CreateTask(null);
            }
            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Command_Cannot_Be_Null", ex.Message);
            }
        }
    }
}
