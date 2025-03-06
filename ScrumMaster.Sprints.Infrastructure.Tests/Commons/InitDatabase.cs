using Moq;
using ScrumMaster.Sprints.Core.Models;
using ScrumMaster.Sprints.Infrastructure.DataAccess;

namespace ScrumMaster.Sprints.Infrastructure.Tests.Commons
{
    internal static class InitDatabase
    {
        internal static ISprintDbContext InitialDatabase()
        {
            List<Sprint> sprints = new List<Sprint>()
            {
                new Sprint("TestName",DateTime.Now.AddDays(-10),DateTime.Now.AddDays(10),"TestCreatedBy",Guid.Parse("00000000-0000-0000-0000-000000000001")){Id = Guid.Parse("00000000-0000-0000-0000-000000000002")},
            };
            var sprintsDbSet = GenericDbSet.GetDbSet(sprints);
            var _context = new Mock<ISprintDbContext>();

            _context.Setup(x => x.Sprints).Returns(sprintsDbSet.Object);

            return _context.Object;
        }
    }
}
