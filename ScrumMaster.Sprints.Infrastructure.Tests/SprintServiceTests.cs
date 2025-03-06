using ScrumMaster.Sprints.Core.Models;
using ScrumMaster.Sprints.Infrastructure.Commands;
using ScrumMaster.Sprints.Infrastructure.Implementations;
using ScrumMaster.Sprints.Infrastructure.Tests.Commons;

namespace ScrumMaster.Sprints.Infrastructure.Tests
{
    public class SprintServiceTests
    {
        [Fact]
        public async Task CreateNewSprintAsync_WhenCommandNull_Should_ThrowException()
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
                Assert.Equal("Command_Cannot_Be_Null", ex.Message);
            }
        }
        [Fact]
        public async Task CreateNewSprintAsync_WhenValidModel_Should_CreateNewSprint()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);

            var result = await service.CreateNewSprintAsync(new Commands.CreateSprintCommand()
            {
                CreatedBy = "test",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                CreatedUserId = Guid.NewGuid(),
                Name = "test",
            });

            Assert.Equal(2, dbContext.Sprints.Count());
        }
        [Fact]
        public async Task UpdateSprintAsync_WhenCommandNull_Should_ThrowException()
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
        [Fact]
        public async Task UpdateSprintAsync_WhenCannotFindSprint_Should_ThrowException()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);

            //Act
            try
            {
                UpdateSprintCommand command = new UpdateSprintCommand();
                command.SprintId = Guid.Parse("00000000-0000-0000-0000-000000000001");
                await service.UpdateSprintAsync(command);
            }

            //Assert
            catch (Exception ex)
            {
                Assert.Equal("Cannot_Find_Sprint_In_Database", ex.Message);
            }
        }
        [Fact]
        public async Task UpdateSprintAsync_WhenStartDateIsTheSameAsEndDate_Should_ThrowException()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);
            UpdateSprintCommand command = new UpdateSprintCommand();
            command.SprintId = Guid.Parse("00000000-0000-0000-0000-000000000002");
            command.StartAt = DateTime.Now;
            command.EndAt = DateTime.Now;

            //Act
            try
            {
                await service.UpdateSprintAsync(command);
            }

            //Assert
            catch (Exception ex)
            {
                Assert.Equal("StartDate_Cannot_Be_The_Same_Or_After_As_EndDate", ex.Message);
            }
        }
        [Fact]
        public async Task UpdateSprintAsync_WhenTryChangeEndDateBeforeStartDate_Should_ThrowException()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);
            UpdateSprintCommand command = new UpdateSprintCommand();
            command.SprintId = Guid.Parse("00000000-0000-0000-0000-000000000002");
            command.EndAt = DateTime.Now.AddDays(-20);

            //Act
            try
            {
                await service.UpdateSprintAsync(command);
            }

            //Assert
            catch (Exception ex)
            {
                Assert.Equal("EndDate_Cannot_Be_The_Same_Or_After_As_StartDate", ex.Message);
            }
        }
        [Fact]
        public async Task UpdateSprintAsync_WhenCommandHasntChanges_Should_ThrowException()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);
            UpdateSprintCommand command = new UpdateSprintCommand();
            command.SprintId = Guid.Parse("00000000-0000-0000-0000-000000000002");

            //Act
            try
            {
                await service.UpdateSprintAsync(command);
            }

            //Assert
            catch (Exception ex)
            {
                Assert.Equal("There_are_no_changes_for_sprint", ex.Message);
            }
        }
        [Fact]
        public async Task UpdateSprintAsync_WhenCommandHasChanges_Should_ChangeProperty()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);
            UpdateSprintCommand command = new UpdateSprintCommand();
            command.SprintId = Guid.Parse("00000000-0000-0000-0000-000000000002");
            command.SprintName = "NameTest";

            //Act
            await service.UpdateSprintAsync(command);
            var sprint = dbContext.Sprints.FirstOrDefault(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000002"));

            //Assert
            Assert.Equal("NameTest", sprint.Name);
        }
        [Fact]
        public async Task DeleteSprintAsync_WhenCannotFindSprint_Should_ThrowException()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);

            //Act
            try
            {
                await service.DeleteSprintAsync(Guid.Parse("00000000-0000-0000-0000-000000000002"));
            }

            //Assert
            catch (Exception ex)
            {
                Assert.Equal("Cannot_Find_Sprint_In_Database", ex.Message);
            }
        }
        [Fact]
        public async Task DeleteSprintAsync_WhenIdIsCorrect_Should_DeleteSprint()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);

            //Act
            await service.DeleteSprintAsync(Guid.Parse("00000000-0000-0000-0000-000000000002"));

            //Assert
            Assert.Equal(0, dbContext.Sprints.Count());
        }
        [Fact]
        public async Task GetSprintByIdAsync_WhenSprintDoesntExist_Should_ReturnNull()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);

            //Act
            var result = await service.GetSprintByIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));

            //Assert
            Assert.True(result == null);
        }
        [Fact]
        public async Task GetSprintByIdAsync_WhenSprintExist_Should_ReturnSprint()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);

            //Act
            var result = await service.GetSprintByIdAsync(Guid.Parse("00000000-0000-0000-0000-000000000002"));

            //Assert
            Assert.Equal("TestName", result.sprintName);
        }
        [Fact]
        public async Task GetAllUserSprintsAsync_WhenSprintsDoesntExist_Should_ReturnNull()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);

            //Act
            var result = await service.GetAllUserSprintsAsync(Guid.Parse("00000000-0000-0000-0000-000000000002"));

            //Assert
            Assert.Equal(0,result.Count());
        }
        [Fact]
        public async Task GetAllUserSprintsAsync_WhenSprintExist_Should_ReturnAllSprints()
        {
            //Arrange
            var dbContext = InitDatabase.InitialDatabase();
            var service = new SprintService(dbContext);

            //Act
            var result = await service.GetAllUserSprintsAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));

            //Assert
            Assert.Equal(1, result.Count());
        }
    }
}
