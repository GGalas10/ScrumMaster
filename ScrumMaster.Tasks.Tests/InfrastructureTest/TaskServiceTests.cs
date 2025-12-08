using Microsoft.Extensions.Configuration;
using ScrumMaster.Tasks.Infrastructure.Commands;
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
            Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
            {"Api.Sprint", "Test"} };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var dbContext = DbContextInitializer.InitDatabase();
            var service = new TaskService(dbContext, ServiceInterfacesInit.CreateSprintAPIService(), ServiceInterfacesInit.CreateProjectAPIService(), ServiceInterfacesInit.CreateUserAPIService());

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
        [Fact]
        public async Task CreateTask_WhenTitleIsNull_Should_ThrowException()
        {
            //Arrange
            Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
            {"Api.Sprint", "Test"} };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var dbContext = DbContextInitializer.InitDatabase();
            var service = new TaskService(dbContext, ServiceInterfacesInit.CreateSprintAPIService(), ServiceInterfacesInit.CreateProjectAPIService(), ServiceInterfacesInit.CreateUserAPIService());
            var command = new CreateTaskCommand() { title = null, description = "TestDescription", sprintId = Guid.NewGuid() };

            //Act
            try
            {
                await service.CreateTask(command);
            }
            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Title_Cannot_Be_Null_Or_Empty", ex.Message);
            }
        }
        [Fact]
        public async Task CreateTask_WhenDescriptionIsNull_Should_ThrowException()
        {
            //Arrange
            Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
            {"Api.Sprint", "Test"} };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var dbContext = DbContextInitializer.InitDatabase();
            var service = new TaskService(dbContext, ServiceInterfacesInit.CreateSprintAPIService(), ServiceInterfacesInit.CreateProjectAPIService(), ServiceInterfacesInit.CreateUserAPIService());
            var command = new CreateTaskCommand() { title = "TestTitle", description = "TestDescription", sprintId = Guid.NewGuid() };

            //Act
            try
            {
                await service.CreateTask(command);
            }
            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Description_Cannot_Be_Null_Or_Empty", ex.Message);
            }
        }
        [Fact]
        public async Task CreateTask_WhenSprintIdIsEmpty_Should_ThrowException()
        {
            //Arrange
            Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
            {"Api.Sprint", "Test"} };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var dbContext = DbContextInitializer.InitDatabase();
            var service = new TaskService(dbContext, ServiceInterfacesInit.CreateSprintAPIService(), ServiceInterfacesInit.CreateProjectAPIService(), ServiceInterfacesInit.CreateUserAPIService());
            var command = new CreateTaskCommand() { title = "TestTitle", description = "TestDescription", sprintId = Guid.Empty };

            //Act
            try
            {
                await service.CreateTask(command);
            }
            //Assert
            catch(Exception ex)
            {
                Assert.Equal("SprintId_Cannot_Be_Empty", ex.Message);
            }
        }
        [Fact]
        public async Task CreateTask_WhenCommandIsCorrect_Should_AddNewTask()
        {
            //Arrange
            Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
            {"Api.Sprint", "Test"} };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var dbContext = DbContextInitializer.InitDatabase();
            var service = new TaskService(dbContext, ServiceInterfacesInit.CreateSprintAPIService(), ServiceInterfacesInit.CreateProjectAPIService(), ServiceInterfacesInit.CreateUserAPIService());
            var command = new CreateTaskCommand() { title = "TestTitle", description = "TestDescription", sprintId = Guid.NewGuid() };

            //Act
            var result = await service.CreateTask(command);

            //Assert
            Assert.Equal(4,dbContext.Tasks.Count());
        }
        [Fact]
        public async Task UpdateTask_WhenCommandIsNull_Should_ThrowException()
        {
            //Arrange
            Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
            {"Api.Sprint", "Test"} };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var dbContext = DbContextInitializer.InitDatabase();
            var service = new TaskService(dbContext, ServiceInterfacesInit.CreateSprintAPIService(), ServiceInterfacesInit.CreateProjectAPIService(), ServiceInterfacesInit.CreateUserAPIService());

            //Act
            try
            {
                await service.UpdateTask(null, Guid.NewGuid());
            }
            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Command_Cannot_Be_Null", ex.Message);
            }
        }
        [Fact]
        public async Task UpdateTask_WhenCannotFindTask_Should_ThrowException()
        {
            //Arrange
            Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
            {"Api.Sprint", "Test"} };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var dbContext = DbContextInitializer.InitDatabase();
            var service = new TaskService(dbContext, ServiceInterfacesInit.CreateSprintAPIService(), ServiceInterfacesInit.CreateProjectAPIService(), ServiceInterfacesInit.CreateUserAPIService());
            var command = new UpdateTaskCommand() {sprintId = Guid.Parse("00000000-0000-0000-0000-000000000001") };

            //Act
            try
            {
                await service.UpdateTask(command, Guid.NewGuid());
            }
            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Cannot_Find_Task_To_Update", ex.Message);
            }
        }
        [Fact]
        public async Task UpdateTask_WhenCommandDoNoTHasAnyChanges_Should_ThrowException()
        {
            //Arrange
            Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
            {"Api.Sprint", "Test"} };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var dbContext = DbContextInitializer.InitDatabase();
            var service = new TaskService(dbContext, ServiceInterfacesInit.CreateSprintAPIService(), ServiceInterfacesInit.CreateProjectAPIService(), ServiceInterfacesInit.CreateUserAPIService());
            var randomTask = dbContext.Tasks.First();
            var command = new UpdateTaskCommand() {oldTaskId = randomTask.Id };

            //Act
            try
            {
                await service.UpdateTask(command, Guid.NewGuid());
            }
            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Any_Changes_To_Change", ex.Message);
            }
        }
        [Fact]
        public async Task UpdateTask_WhenCommandHasNewTitle_Should_ChangeTitle()
        {
            //Arrange
            Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
            {"Api.Sprint", "Test"} };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var dbContext = DbContextInitializer.InitDatabase();
            var service = new TaskService(dbContext, ServiceInterfacesInit.CreateSprintAPIService(), ServiceInterfacesInit.CreateProjectAPIService(), ServiceInterfacesInit.CreateUserAPIService());
            var randomTask = dbContext.Tasks.First();
            var command = new UpdateTaskCommand() {oldTaskId = randomTask.Id,title = "NewTestTitleAfterUpdate" };

            //Act
             await service.UpdateTask(command, Guid.NewGuid());
            var updatedTask = dbContext.Tasks.FirstOrDefault(x => x.Id == randomTask.Id);

            //Assert
            Assert.Equal("NewTestTitleAfterUpdate", updatedTask.Title);
        }
        [Fact]
        public async Task DeleteTask_WhenGuidIsEmpty_Should_ThrowException()
        {
            //Arrange
            Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
            {"Api.Sprint", "Test"} };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var dbContext = DbContextInitializer.InitDatabase();
            var service = new TaskService(dbContext, ServiceInterfacesInit.CreateSprintAPIService(), ServiceInterfacesInit.CreateProjectAPIService(), ServiceInterfacesInit.CreateUserAPIService());

            //Act
            try
            {
                await service.DeleteTask(Guid.Empty, Guid.NewGuid());
            }

            //Assert
            catch(Exception ex)
            {
                Assert.Equal("TaskId_Cannot_Be_Empty", ex.Message);
            }
        }
        [Fact]
        public async Task DeleteTask_WhenCannotFindTask_Should_ThrowException()
        {
            //Arrange
            Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
            {"Api.Sprint", "Test"} };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var dbContext = DbContextInitializer.InitDatabase();
            var service = new TaskService(dbContext, ServiceInterfacesInit.CreateSprintAPIService(), ServiceInterfacesInit.CreateProjectAPIService(), ServiceInterfacesInit.CreateUserAPIService());

            //Act
            try
            {
                await service.DeleteTask(Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.NewGuid());
            }

            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Cannot_Find_Task_To_Delete", ex.Message);
            }
        }
        [Fact]
        public async Task DeleteTask_WhenTaskIdIsCorrect_Should_RemoveTask()
        {
            //Arrange
            Dictionary<string, string> inMemorySettings = new Dictionary<string, string> {
            {"Api.Sprint", "Test"} };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var dbContext = DbContextInitializer.InitDatabase();
            var service = new TaskService(dbContext, ServiceInterfacesInit.CreateSprintAPIService(), ServiceInterfacesInit.CreateProjectAPIService(), ServiceInterfacesInit.CreateUserAPIService());
            var randomTask = dbContext.Tasks.First();

            //Act
            await service.DeleteTask(randomTask.Id, Guid.NewGuid());

            //Assert

            Assert.Equal(2, dbContext.Tasks.Count());
        }
    }
}
