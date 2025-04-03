using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;

namespace ScrumMaster.Identity.Tests.Common
{
    internal class AsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public AsyncEnumerable(IEnumerable<T> enumerable)
                : base(enumerable)
        { }
        public AsyncEnumerable(Expression expression)
                : base(expression)
        { }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }
        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }
        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }
        IQueryProvider IQueryable.Provider => new AsyncQueryProvider<T>(this);
    }
}
