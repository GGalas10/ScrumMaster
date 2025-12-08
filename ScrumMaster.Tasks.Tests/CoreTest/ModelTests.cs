using ScrumMaster.Tasks.Core.Models;

namespace ScrumMaster.Tasks.Tests.CoreTest
{
    public class ModelTests
    {
        [Fact]
        public void SetTitle_WhenNewTitleIsEmpty_Should_ThrowException()
        {
            //Arrange
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Empty, "");
            //Act
            try
            {
                newTask.SetTitle(null);
            }

            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Title_Cannot_Be_Null_Or_Empty", ex.Message);
            }
        }
        [Fact]
        public void SetTitle_WhenNewTittleIsCorrect_Should_ChangeModelTitle() 
        {
            //Arrange
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Empty, "");
            //Act
            newTask.SetTitle("NewTestTitle");
            //Assert
            Assert.Equal("NewTestTitle", newTask.Title);
        }
        [Fact]
        public void SetDescription_WhenNewTitleIsEmpty_Should_ThrowException()
        {
            //Arrange
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Empty, "");
            //Act
            try
            {
                newTask.SetDescription(null);
            }

            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Description_Cannot_Be_Null_Or_Empty", ex.Message);
            }
        }
        [Fact]
        public void SetDescription_WhenNewDescriptionIsCorrect_Should_ChangeModelDescription()
        {
            //Arrange
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Empty, "");
            //Act
            newTask.SetDescription("NewTestDescription");
            //Assert
            Assert.Equal("NewTestDescription", newTask.Description);
        }
        [Fact]
        public void ChangeAssignedUser_WhenNewUserIdIsEmpty_Should_ThrowException()
        {
            //Arrange
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Empty, "");
            //Act
            try
            {
                newTask.ChangeAssignedUser(Guid.Empty);
            }

            //Assert
            catch (Exception ex)
            {
                Assert.Equal("UserId_Cannot_Be_Empty", ex.Message);
            }
        }
        [Fact]
        public void ChangeAssignedUser_WhenNewUserIdIsCorrect_Should_ChangeModelUserId()
        {
            //Arrange
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Empty, "");
            //Act
            newTask.ChangeAssignedUser(Guid.Parse("00000000-0000-0000-0000-000000000002"));

            //Assert
            Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000002"), newTask.AssignedUserId);
        }
        [Fact]
        public void ChangeSprint_WhenNewSprintIdIsEmpty_Should_ThrowException()
        {
            //Arrange
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Empty, "");
            //Act
            try
            {
                newTask.ChangeSprint(Guid.Empty);
            }

            //Assert
            catch (Exception ex)
            {
                Assert.Equal("SprintId_Cannot_Be_Empty", ex.Message);
            }
        }
        [Fact]
        public void ChangeSprint_WhenNewSprintIdIsCorrect_Should_ChangeModelSprintId()
        {
            //Arrange
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Empty, "");
            //Act
            newTask.ChangeSprint(Guid.Parse("00000000-0000-0000-0000-000000000002"));

            //Assert
            Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000002"), newTask.SprintId);
        }
        [Fact]
        public void UpdateTask_WhenHasNotAnyChanges_Should_ReturnFalse()
        {
            //Arrange
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Empty, "");
            //Act
            var anyChanges =  newTask.UpdateTask(null,null,Core.Enums.StatusEnum.New,Guid.Empty,Guid.Empty, "");

            //Assert
            Assert.False(anyChanges);
        }
        [Fact]
        public void UpdateTask_WhenHasAnyChanges_Should_ReturnTrue()
        {
            //Arrange
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Empty, "");
            //Act
            var anyChanges =  newTask.UpdateTask("NewTestName",null,Core.Enums.StatusEnum.New,Guid.Empty,Guid.Empty, "");

            //Assert
            Assert.True(anyChanges);
        }
        [Fact]
        public void UpdateTask_WhenGetNewName_Should_ChangeName()
        {
            //Arrange
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Empty, "");
            //Act
            var anyChanges =  newTask.UpdateTask("NewTestName",null,Core.Enums.StatusEnum.New,Guid.Empty,Guid.Empty, "");

            //Assert
            Assert.Equal("NewTestName",newTask.Title);
        }
    }
}
