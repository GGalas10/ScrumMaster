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
            };
            var sprintsDbSet = GenericDbSet.GetDbSet(sprints);
            var _context = new Mock<ISprintDbContext>();

            _context.Setup(x => x.Sprints).Returns(sprintsDbSet.Object);

            return _context.Object;
        }
    }
}
