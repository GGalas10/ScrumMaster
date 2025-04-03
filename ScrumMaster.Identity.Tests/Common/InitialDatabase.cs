using Moq;
using ScrumMaster.Identity.Core.Models;
using ScrumMaster.Identity.Infrastructure.DataAccesses;

namespace ScrumMaster.Identity.Tests.Common
{
    internal class InitialDatabase
    {
        internal static IUserDbContext InitDatabase()
        {
            List<RefreshToken> sprints = new List<RefreshToken>()
            {
                new RefreshToken(){Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),Token = "Test"},
            };
            var sprintsDbSet = GenericDbSet.GetDbSet(sprints);
            var _context = new Mock<IUserDbContext>();

            _context.Setup(x => x.RefreshTokens).Returns(sprintsDbSet.Object);

            return _context.Object;
        }
    }
}
