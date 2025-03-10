using Moq;
using ScrumMaster.Tasks.Core.Models;
using ScrumMaster.Tasks.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumMaster.Tasks.Tests.InfrastructureTest.Commons
{
    internal class DbContextInitializer
    {
        internal static ITaskDbContext InitDatabase()
        {
            List<TaskModel> sprints = new List<TaskModel>()
            {
                new TaskModel("TestTitle1","TestDescription1",Guid.Parse("00000000-0000-0000-0000-000000000001"),Guid.Parse("00000000-0000-0000-0000-000000000001")),
                new TaskModel("TestTitle2","TestDescription2",Guid.Parse("00000000-0000-0000-0000-000000000002"),Guid.Parse("00000000-0000-0000-0000-000000000001")),
                new TaskModel("TestTitle3","TestDescription3",Guid.Parse("00000000-0000-0000-0000-000000000001"),Guid.Parse("00000000-0000-0000-0000-000000000002")),
            };
            var tasksDbSet = GenericDbSet.GetDbSet(sprints);
            var _context = new Mock<ITaskDbContext>();

            _context.Setup(x => x.Tasks).Returns(tasksDbSet.Object);

            return _context.Object;
        }
    }
}
