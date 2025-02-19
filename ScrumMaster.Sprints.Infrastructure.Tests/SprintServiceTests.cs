using ScrumMaster.Sprints.Infrastructure.Implementations;
using ScrumMaster.Sprints.Infrastructure.Tests.Commons;

namespace ScrumMaster.Sprints.Infrastructure.Tests
{
    public class SprintServiceTests
    {
        [Fact]
        public async void CreateNewSprintAsync_WhenCommandNull_Should_ThrowException()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);

            //Act
            try
            {
                var result = await service.CreateNewSprintAsync(null);
            }

            //Assert
            catch (Exception ex)
            {
                Assert.Equal("Command_Cannot_Be_Null",ex.Message);
            }
        }
        [Fact]
        public async void CreateNewSprintAsync_WhenValidModel_Should_CreateNewSprint()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);

            var result = await service.CreateNewSprintAsync(new Commands.CreateSprintCommand()
            {
                CreatedBy = "test",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CreatedUserId = Guid.NewGuid(),
                Name = "test",
            });

            Assert.Equal(1,dbContext.Sprints.Count());
        }
        [Fact]
        public async void UpdateSprintAsync_WhenCommandNull_Should_ThrowException()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);

            //Act
            try
            {
                await service.UpdateSprintAsync(null);
            }

            //Assert
            catch (Exception ex)
            {
                Assert.Equal("Command_Cannot_Be_Null", ex.Message);
            }
        }
    }
}
