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
                new Sprint(){Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),CreatedBy = "TestCreatedBy",CreatedUserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),Name = "TestName",StartDate = DateTime.Now.AddDays(-10),EndDate = DateTime.Now.AddDays(10)},
            };
            var sprintsDbSet = GenericDbSet.GetDbSet(sprints);
            var _context = new Mock<ISprintDbContext>();

            _context.Setup(x => x.Sprints).Returns(sprintsDbSet.Object);

            return _context.Object;
        }
    }
}
