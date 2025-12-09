using Microsoft.Extensions.Configuration;
using ScrumMaster.Tasks.Infrastructure.Commands;
using ScrumMaster.Tasks.Infrastructure.Implementations;
using ScrumMaster.Tasks.Infrastructure.Exceptions;
using ScrumMaster.Tasks.Tests.InfrastructureTest.Commons;
using ScrumMaster.Tasks.Infrastructure.DataAccess;

namespace ScrumMaster.Tasks.Tests.InfrastructureTest
{
    public class TaskServiceTests
    {
        private IConfiguration _configuration;
        private TaskService _service;
        private ITaskDbContext _dbContext;

        private void SetupTest()
        {
            Dictionary<string, string> inMemorySettings = new Dictionary<string, string>
            {
                { "Api.Sprint", "Test" }
            };
            _configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            _dbContext = DbContextInitializer.InitDatabase();
            _service = new TaskService(
                _dbContext,
                ServiceInterfacesInit.CreateSprintAPIService(),
                ServiceInterfacesInit.CreateProjectAPIService(),
                ServiceInterfacesInit.CreateUserAPIService()
            );
        }

        #region CreateTask Tests

        [Fact]
        public async Task CreateTask_WhenCommandIsNull_Should_ThrowException()
        {
            //Arrange
            SetupTest();

            //Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateTask(null));
            Assert.Equal("Command_Cannot_Be_Null", exception.Message);
        }

        [Fact]
        public async Task CreateTask_WhenTitleIsNull_Should_ThrowException()
        {
            //Arrange
            SetupTest();
            var command = new CreateTaskCommand
            {
                title = null,
                description = "TestDescription",
                sprintId = Guid.NewGuid(),
                createdById = Guid.NewGuid()
            };

            //Act & Assert
            var exception = await Assert.ThrowsAsync<Core.Exceptions.BadRequestException>(() => _service.CreateTask(command));
            Assert.Equal("Title_Cannot_Be_Null_Or_Empty", exception.Message);
        }

        [Fact]
        public async Task CreateTask_WhenTitleIsEmpty_Should_ThrowException()
        {
            //Arrange
            SetupTest();
            var command = new CreateTaskCommand
            {
                title = string.Empty,
                description = "TestDescription",
                sprintId = Guid.NewGuid(),
                createdById = Guid.NewGuid()
            };

            //Act & Assert
            var exception = await Assert.ThrowsAsync<Core.Exceptions.BadRequestException>(() => _service.CreateTask(command));
            Assert.Equal("Title_Cannot_Be_Null_Or_Empty", exception.Message);
        }

        [Fact]
        public async Task CreateTask_WhenDescriptionIsNull_Should_ThrowException()
        {
            //Arrange
            SetupTest();
            var command = new CreateTaskCommand
            {
                title = "TestTitle",
                description = null,
                sprintId = Guid.NewGuid(),
                createdById = Guid.NewGuid()
            };

            //Act & Assert
            var exception = await Assert.ThrowsAsync<Core.Exceptions.BadRequestException>(() => _service.CreateTask(command));
            Assert.Equal("Description_Cannot_Be_Null_Or_Empty", exception.Message);
        }

        [Fact]
        public async Task CreateTask_WhenDescriptionIsEmpty_Should_ThrowException()
        {
            //Arrange
            SetupTest();
            var command = new CreateTaskCommand
            {
                title = "TestTitle",
                description = string.Empty,
                sprintId = Guid.NewGuid(),
                createdById = Guid.NewGuid()
            };

            //Act & Assert
            var exception = await Assert.ThrowsAsync<Core.Exceptions.BadRequestException>(() => _service.CreateTask(command));
            Assert.Equal("Description_Cannot_Be_Null_Or_Empty", exception.Message);
        }

        [Fact]
        public async Task CreateTask_WhenSprintIdIsEmpty_Should_ThrowException()
        {
            //Arrange
            SetupTest();
            var command = new CreateTaskCommand
            {
                title = "TestTitle",
                description = "TestDescription",
                sprintId = Guid.Empty,
                createdById = Guid.NewGuid()
            };

            //Act & Assert
            var exception = await Assert.ThrowsAsync<Core.Exceptions.BadRequestException>(() => _service.CreateTask(command));
            Assert.Equal("SprintId_Cannot_Be_Empty", exception.Message);
        }

        [Fact]
        public async Task CreateTask_WhenCreatedByIdIsEmpty_Should_ThrowException()
        {
            //Arrange
            SetupTest();
            var command = new CreateTaskCommand
            {
                title = "TestTitle",
                description = "TestDescription",
                sprintId = Guid.NewGuid(),
                createdById = Guid.Empty
            };

            //Act & Assert
            var exception = await Assert.ThrowsAsync<Core.Exceptions.BadRequestException>(() => _service.CreateTask(command));
            Assert.Equal("CreatedById_Cannot_Be_Empty", exception.Message);
        }

        [Fact]
        public async Task CreateTask_WhenCommandIsValid_Should_ReturnTaskId()
        {
            //Arrange
            SetupTest();
            var userId = Guid.NewGuid();
            var sprintId = Guid.NewGuid();
            var command = new CreateTaskCommand
            {
                title = "NewTask",
                description = "New Description",
                sprintId = sprintId,
                createdById = userId
            };

            //Act
            var result = await _service.CreateTask(command);

            //Assert
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(4, _dbContext.Tasks.Count());
        }

        #endregion

        #region UpdateTask Tests

        [Fact]
        public async Task UpdateTask_WhenCommandIsNull_Should_ThrowException()
        {
            //Arrange
            SetupTest();

            //Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.UpdateTask(null, Guid.NewGuid()));
            Assert.Equal("Command_Cannot_Be_Null", exception.Message);
        }

        [Fact]
        public async Task UpdateTask_WhenTaskNotFound_Should_ThrowException()
        {
            //Arrange
            SetupTest();
            var command = new UpdateTaskCommand
            {
                oldTaskId = Guid.NewGuid(),
                sprintId = Guid.NewGuid()
            };

            //Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.UpdateTask(command, Guid.NewGuid()));
            Assert.Equal("Cannot_Find_Task_To_Update", exception.Message);
        }

        [Fact]
        public async Task UpdateTask_WhenNoChangesProvided_Should_ThrowException()
        {
            //Arrange
            SetupTest();
            var randomTask = _dbContext.Tasks.First();
            var command = new UpdateTaskCommand
            {
                oldTaskId = randomTask.Id,
                sprintId = randomTask.SprintId
            };

            //Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.UpdateTask(command, Guid.NewGuid()));
            Assert.Equal("Any_Changes_To_Change", exception.Message);
        }

        [Fact]
        public async Task UpdateTask_WhenUpdatingTitle_Should_ChangeTitle()
        {
            //Arrange
            SetupTest();
            var randomTask = _dbContext.Tasks.First();
            var newTitle = "UpdatedTaskTitle";
            var command = new UpdateTaskCommand
            {
                oldTaskId = randomTask.Id,
                title = newTitle,
                sprintId = randomTask.SprintId
            };

            //Act
            await _service.UpdateTask(command, Guid.NewGuid());
            var updatedTask = _dbContext.Tasks.FirstOrDefault(x => x.Id == randomTask.Id);

            //Assert
            Assert.Equal(newTitle, updatedTask.Title);
        }

        [Fact]
        public async Task UpdateTask_WhenUpdatingDescription_Should_ChangeDescription()
        {
            //Arrange
            SetupTest();
            var randomTask = _dbContext.Tasks.First();
            var newDescription = "UpdatedDescription";
            var command = new UpdateTaskCommand
            {
                oldTaskId = randomTask.Id,
                description = newDescription,
                sprintId = randomTask.SprintId
            };

            //Act
            await _service.UpdateTask(command, Guid.NewGuid());
            var updatedTask = _dbContext.Tasks.FirstOrDefault(x => x.Id == randomTask.Id);

            //Assert
            Assert.Equal(newDescription, updatedTask.Description);
        }

        [Fact]
        public async Task UpdateTask_WhenUpdatingStatus_Should_ChangeStatus()
        {
            //Arrange
            SetupTest();
            var randomTask = _dbContext.Tasks.First();
            var command = new UpdateTaskCommand
            {
                oldTaskId = randomTask.Id,
                status = Core.Enums.StatusEnum.Active,
                sprintId = randomTask.SprintId
            };

            //Act
            await _service.UpdateTask(command, Guid.NewGuid());
            var updatedTask = _dbContext.Tasks.FirstOrDefault(x => x.Id == randomTask.Id);

            //Assert
            Assert.Equal(Core.Enums.StatusEnum.Active, updatedTask.Status);
        }

        [Fact]
        public async Task UpdateTask_WhenAssigningUser_Should_AssignUser()
        {
            //Arrange
            SetupTest();
            var randomTask = _dbContext.Tasks.First();
            var assignedUserId = Guid.NewGuid();
            var command = new UpdateTaskCommand
            {
                oldTaskId = randomTask.Id,
                assignedUserId = assignedUserId,
                sprintId = randomTask.SprintId
            };

            //Act
            await _service.UpdateTask(command, Guid.NewGuid());
            var updatedTask = _dbContext.Tasks.FirstOrDefault(x => x.Id == randomTask.Id);

            //Assert
            Assert.Equal(assignedUserId, updatedTask.AssignedUserId);
        }

        [Fact]
        public async Task UpdateTask_WhenUpdatingMultipleFields_Should_UpdateAll()
        {
            //Arrange
            SetupTest();
            var randomTask = _dbContext.Tasks.First();
            var newTitle = "MultiUpdateTitle";
            var newDescription = "MultiUpdateDescription";
            var assignedUserId = Guid.NewGuid();
            var command = new UpdateTaskCommand
            {
                oldTaskId = randomTask.Id,
                title = newTitle,
                description = newDescription,
                status = Core.Enums.StatusEnum.Complete,
                assignedUserId = assignedUserId,
                sprintId = randomTask.SprintId
            };

            //Act
            await _service.UpdateTask(command, Guid.NewGuid());
            var updatedTask = _dbContext.Tasks.FirstOrDefault(x => x.Id == randomTask.Id);

            //Assert
            Assert.Equal(newTitle, updatedTask.Title);
            Assert.Equal(newDescription, updatedTask.Description);
            Assert.Equal(Core.Enums.StatusEnum.Complete, updatedTask.Status);
            Assert.Equal(assignedUserId, updatedTask.AssignedUserId);
        }

        #endregion

        #region DeleteTask Tests

        [Fact]
        public async Task DeleteTask_WhenTaskIdIsEmpty_Should_ThrowException()
        {
            //Arrange
            SetupTest();

            //Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.DeleteTask(Guid.Empty, Guid.NewGuid()));
            Assert.Equal("TaskId_Cannot_Be_Empty", exception.Message);
        }

        [Fact]
        public async Task DeleteTask_WhenTaskNotFound_Should_ThrowException()
        {
            //Arrange
            SetupTest();

            //Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.DeleteTask(Guid.NewGuid(), Guid.NewGuid()));
            Assert.Equal("Cannot_Find_Task_To_Delete", exception.Message);
        }

        [Fact]
        public async Task DeleteTask_WhenTaskExists_Should_RemoveTask()
        {
            //Arrange
            SetupTest();
            var taskToDelete = _dbContext.Tasks.First();
            var initialCount = _dbContext.Tasks.Count();

            //Act
            await _service.DeleteTask(taskToDelete.Id, Guid.NewGuid());

            //Assert
            Assert.Equal(initialCount - 1, _dbContext.Tasks.Count());
            Assert.Null(_dbContext.Tasks.FirstOrDefault(x => x.Id == taskToDelete.Id));
        }

        #endregion

        #region GetTaskById Tests

        [Fact]
        public async Task GetTaskById_WhenTaskIdIsEmpty_Should_ThrowException()
        {
            //Arrange
            SetupTest();

            //Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.GetTaskById(Guid.Empty, Guid.NewGuid()));
            Assert.Equal("TaskId_Cannot_Be_Empty", exception.Message);
        }

        [Fact]
        public async Task GetTaskById_WhenTaskNotFound_Should_ThrowException()
        {
            //Arrange
            SetupTest();

            //Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.GetTaskById(Guid.NewGuid(), Guid.NewGuid()));
            Assert.Equal("Cannot_Find_Task", exception.Message);
        }

        [Fact]
        public async Task GetTaskById_WhenTaskExists_Should_ReturnTaskDTO()
        {
            //Arrange
            SetupTest();
            var task = _dbContext.Tasks.First();

            //Act
            var result = await _service.GetTaskById(task.Id, Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(task.Title, result.Title);
            Assert.Equal(task.Description, result.Description);
        }

        #endregion

        #region GetAllSprintTasks Tests

        [Fact]
        public async Task GetAllSprintTasks_WhenSprintIdIsEmpty_Should_ThrowException()
        {
            //Arrange
            SetupTest();

            //Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _service.GetAllSprintTasks(Guid.Empty, Guid.NewGuid()));
            Assert.Equal("SprintId_Cannot_Be_Empty", exception.Message);
        }

        [Fact]
        public async Task GetAllSprintTasks_WhenSprintHasTasks_Should_ReturnAllTasks()
        {
            //Arrange
            SetupTest();
            var sprintId = _dbContext.Tasks.First().SprintId;

            //Act
            var result = await _service.GetAllSprintTasks(sprintId, Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.All(result, task => Assert.Equal(sprintId, task.SprintId));
        }

        [Fact]
        public async Task GetAllSprintTasks_WhenSprintHasNoTasks_Should_ReturnEmptyList()
        {
            //Arrange
            SetupTest();
            var emptySprintId = Guid.NewGuid();

            //Act
            var result = await _service.GetAllSprintTasks(emptySprintId, Guid.NewGuid());

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetTaskStatuses Tests

        [Fact]
        public void GetTaskStatuses_Should_ReturnAllStatuses()
        {
            //Arrange
            SetupTest();

            //Act
            var result = _service.GetTaskStatuses();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void GetTaskStatuses_Should_NotIncludeUnknownStatus()
        {
            //Arrange
            SetupTest();

            //Act
            var result = _service.GetTaskStatuses();

            //Assert
            Assert.DoesNotContain(result, x => x.statusName == "Unknown");
        }

        [Fact]
        public void GetTaskStatuses_Should_HaveCorrectStructure()
        {
            //Arrange
            SetupTest();

            //Act
            var result = _service.GetTaskStatuses();

            //Assert
            Assert.All(result, status =>
            {
                Assert.NotNull(status.statusName);
                Assert.NotEmpty(status.statusName);
                Assert.True(status.statusOrder >= 0);
            });
        }

        #endregion
    }
}
