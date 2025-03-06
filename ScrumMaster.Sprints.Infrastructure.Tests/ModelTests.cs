using ScrumMaster.Sprints.Core.Models;

namespace ScrumMaster.Sprints.Infrastructure.Tests
{
    public class ModelTests
    {
        [Fact]
        public void Constructor_WhenNameIsNull_Should_ThrowException()
        {
            //Arrange/Act
            try
            {
                var newSprint = new Sprint(null, DateTime.Now, DateTime.Now.AddDays(10), "TestUser", Guid.NewGuid());
            }
            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Name_Cannot_Be_Empty_Or_Null", ex.Message);
            }
        }
        [Fact]
        public void Constructor_WhenStartDateIsAfterEndDate_Should_ThrowException()
        {
            //Arrange/Act
            try
            {
                var newSprint = new Sprint("Sprint Name", DateTime.Now.AddDays(10), DateTime.Now, "TestUser", Guid.NewGuid());
            }
            //Assert
            catch(Exception ex)
            {
                Assert.Equal("StartDate_Cannot_Be_After_EndDate", ex.Message);
            }
        }
        [Fact]
        public void Constructor_WhenStartDateIsTheSameAsEndDate_Should_ThrowException()
        {
            //Arrange/Act
            try
            {
                var newSprint = new Sprint("Sprint Name", DateTime.Now, DateTime.Now, "TestUser", Guid.NewGuid());
            }
            //Assert
            catch(Exception ex)
            {
                Assert.Equal("StartDate_Cannot_Be_The_Same_As_EndDate", ex.Message);
            }
        }
        [Fact]
        public void Constructor_WhenCreatedByIsNull_Should_ThrowException()
        {
            //Arrange/Act
            try
            {
                var newSprint = new Sprint("Sprint Name", DateTime.Now, DateTime.Now.AddDays(1), null, Guid.NewGuid());
            }
            //Assert
            catch (Exception ex)
            {
                Assert.Equal("CreatedBy_Cannot_Be_Null_Or_Empty", ex.Message);
            }
        }
        [Fact]
        public void Constructor_WhenCreatedUserIdIsEmpty_Should_ThrowException()
        {
            //Arrange/Act
            try
            {
                var newSprint = new Sprint("Sprint Name", DateTime.Now, DateTime.Now.AddDays(1), "CreatedTest", Guid.Empty);
            }
            //Assert
            catch (Exception ex)
            {
                Assert.Equal("CreatedUserId_Cannot_Be_Empty", ex.Message);
            }
        }
        [Fact]
        public void UpdateModel_IfThereIsNoChanges_Should_ReturnFalse()
        {
            //Arrange
            var newSprint = new Sprint("Sprint Name", DateTime.Now, DateTime.Now.AddDays(1), "CreatedTest", Guid.NewGuid());

            //Act
            var result = newSprint.UpdateSprint(null, null, DateTime.MinValue, DateTime.MinValue);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void UpdateModel_IfStartDateIsAfterEnd_Should_ThrowException()
        {
            //Arrange
            var newSprint = new Sprint("Sprint Name", DateTime.Now, DateTime.Now.AddDays(1), "CreatedTest", Guid.NewGuid());

            //Act
            try
            {
                var result = newSprint.UpdateSprint(null, null, DateTime.Now.AddDays(2), DateTime.MinValue);
            }
            //Assert
            catch (Exception ex)
            {
                Assert.Equal("StartDate_Cannot_Be_The_Same_Or_After_As_EndDate", ex.Message);
            }
        }
        [Fact]
        public void UpdateModel_IfStartDateAndEndDateIsTheSame_Should_ThrowException()
        {
            //Arrange
            var newSprint = new Sprint("Sprint Name", DateTime.Now, DateTime.Now.AddDays(1), "CreatedTest", Guid.NewGuid());

            //Act
            try
            {
                var result = newSprint.UpdateSprint(null, null, DateTime.Now.AddDays(2), DateTime.Now.AddDays(2));
            }
            //Assert
            catch (Exception ex)
            {
                Assert.Equal("StartDate_Cannot_Be_The_Same_Or_After_As_EndDate", ex.Message);
            }
        }
        [Fact]
        public void UpdateModel_IfEndDateIsBeforeStartDate_Should_ThrowException()
        {
            //Arrange
            var newSprint = new Sprint("Sprint Name", DateTime.Now, DateTime.Now.AddDays(1), "CreatedTest", Guid.NewGuid());

            //Act
            try
            {
                var result = newSprint.UpdateSprint(null, null, DateTime.Now.AddDays(2), DateTime.Now.AddDays(2));
            }
            //Assert
            catch (Exception ex)
            {
                Assert.Equal("EndDate_Cannot_Be_The_Same_Or_Befor_As_StartDate", ex.Message);
            }
        }
        [Fact]
        public void UpdateModel_IfItHasChanges_Should_ReturnTrue()
        {
            //Arrange
            var newSprint = new Sprint("Sprint Name", DateTime.Now, DateTime.Now.AddDays(1), "CreatedTest", Guid.NewGuid());

            //Act
            var result = newSprint.UpdateSprint("NewTestName", null, DateTime.MinValue, DateTime.MinValue);

            //Assert
            Assert.True(result);
        }
    }
}
