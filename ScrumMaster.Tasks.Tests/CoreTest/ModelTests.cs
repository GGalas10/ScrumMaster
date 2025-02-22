using ScrumMaster.Tasks.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumMaster.Tasks.Tests.CoreTest
{
    public class ModelTests
    {
        [Fact]
        public void SetTitle_WhenNewTitleIsEmpty_Should_ThrowException()
        {
            //Arrange
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Parse("00000000-0000-0000-0000-000000000001"));
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
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Parse("00000000-0000-0000-0000-000000000001"));
            //Act
            newTask.SetTitle("NewTestTitle");
            //Assert
            Assert.Equal("NewTestTitle", newTask.Title);
        }
        [Fact]
        public void SetDescription_WhenNewTitleIsEmpty_Should_ThrowException()
        {
            //Arrange
            var newTask = new TaskModel("TestName", "TestDescription", Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.Parse("00000000-0000-0000-0000-000000000001"));
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
    }
}
