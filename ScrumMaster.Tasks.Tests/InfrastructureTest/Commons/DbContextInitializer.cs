using Moq;
using ScrumMaster.Tasks.Core.Models;
using ScrumMaster.Tasks.Infrastructure.DataAccess;

namespace ScrumMaster.Tasks.Tests.InfrastructureTest.Commons
{
    internal class DbContextInitializer
    {
        internal static ITaskDbContext InitDatabase()
        {
            List<TaskModel> tasks = new List<TaskModel>()
            {
                new TaskModel("TestTitle1","TestDescription1",Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.NewGuid(), "Test"),
                new TaskModel("TestTitle2","TestDescription2",Guid.Parse("00000000-0000-0000-0000-000000000002"), Guid.NewGuid(), "Test Test"),
                new TaskModel("TestTitle3","TestDescription3",Guid.Parse("00000000-0000-0000-0000-000000000001"), Guid.NewGuid(), "123 Test"),
            };
            var tasksDbSet = GenericDbSet.GetDbSet(tasks);
            var _context = new Mock<ITaskDbContext>();

            _context.Setup(x => x.Tasks).Returns(tasksDbSet.Object);
            _context.Setup(x => x.Tasks.Add(It.IsAny<TaskModel>())).Callback<TaskModel>(x => tasks.Add(x));

            return _context.Object;
        }
    }
}
