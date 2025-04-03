using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumMaster.Identity.Tests.Common
{
    internal class GenericDbSet
    {
        internal static Mock<DbSet<T>> GetDbSet<T>(List<T> values) where T : class
        {
            var asyncEnumerable = new AsyncEnumerable<T>(values);
            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new AsyncQueryProvider<T>(asyncEnumerable.AsQueryable().Provider));
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(asyncEnumerable.AsQueryable().Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(asyncEnumerable.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => asyncEnumerable.AsQueryable().GetEnumerator());
            mockDbSet.Setup(x => x.Add(It.IsAny<T>())).Callback<T>((x) => values.Add(x));
            mockDbSet.Setup(x => x.Remove(It.IsAny<T>())).Callback<T>((x) => values.Remove(x));
            mockDbSet.Setup(x => x.RemoveRange(It.IsAny<List<T>>())).Callback<IEnumerable<T>>((x) =>
            {
                foreach (var item in x)
                {
                    values.Remove(item);
                }
            });
            return mockDbSet;
        }
    }
}
