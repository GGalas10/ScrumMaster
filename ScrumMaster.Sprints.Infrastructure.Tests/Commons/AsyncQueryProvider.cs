using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ScrumMaster.Sprints.Infrastructure.Tests.Commons
{
    internal class AsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;
        internal AsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }
        public IQueryable CreateQuery(Expression expression)
        {
            return new AsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new AsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }
        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new AsyncEnumerable<TResult>(expression);
        }
        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            var resultType = typeof(TResult).GetGenericArguments()[0];
            var executeAsyncMethod = typeof(AsyncQueryProvider<TEntity>).GetMethod(nameof(ExecuteAsync), new[] { typeof(Expression), typeof(CancellationToken) }).MakeGenericMethod(resultType);
            return (TResult)executeAsyncMethod.Invoke(this, new object[] { expression, CancellationToken.None });
        }
        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }
}
