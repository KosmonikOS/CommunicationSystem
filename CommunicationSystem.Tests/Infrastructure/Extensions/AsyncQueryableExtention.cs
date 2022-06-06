using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationSystem.Tests.Infrastructure.Extensions
{
    public static class AsyncQueryableExtention
    {
        public static IQueryable<T> AsAsyncQueryable<T>(this IEnumerable<T> enumerable)
            => new AsyncQueyable<T>(enumerable);
    }
    public class AsyncQueyable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>
    {
        public AsyncQueyable(IEnumerable<T> enumerable) : base(enumerable) { }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => new AsyncEnumerator(this.AsEnumerable().GetEnumerator());

        private class AsyncEnumerator : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> inner;
            public AsyncEnumerator(IEnumerator<T> inner)
            {
                this.inner = inner;
            }
            public T Current => inner.Current;
            public async ValueTask DisposeAsync() => inner.Dispose();
            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(inner.MoveNext());
        }
    }
}



