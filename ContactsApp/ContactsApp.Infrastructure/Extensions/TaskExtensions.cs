using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

#nullable enable

namespace ContactsApp.Infrastructure.Extensions
{
    public static class TaskExtensions
    {
        private static readonly ConcurrentDictionary<DateTime, string> CanceledTable = new ConcurrentDictionary<DateTime, string>();
        private readonly struct CanceledInfo
        {
            private readonly string _marker;
            private readonly DateTime _date;

            public readonly string Marker
            {
                get => _marker;
            }
            public readonly DateTime Date
            {
                get => _date;
            }

            public CanceledInfo(string marker)
            {
                _marker = marker;
                _date = DateTime.Now;
            }
        }

        public static Dictionary<DateTime, string> GetCanceledTasksLog()
        {
            var keys = CanceledTable.Keys.ToArray();
            var values = CanceledTable.Values.ToArray();

            var log = new Dictionary<DateTime, string>();

            for (var index = 0; index < keys.Length; index++)
            {
                log.Add(keys[index], values[index]);
            }

            return log;
        }

        /// <summary>use await in calling code(delete comment)</summary>
        public static async Task<TResult> WithCancellation<TResult>(this Task<TResult> @this, CancellationToken token, string marker)
        {
            var canceltask = new TaskCompletionSource<CanceledInfo>();

            using (token.Register(callback: t =>
            {
                var tcs = t as TaskCompletionSource<CanceledInfo>;
                tcs!.TrySetResult(new CanceledInfo(marker));
                CanceledTable.TryAdd(tcs.Task.Result.Date, tcs.Task.Result.Marker);
            }, state: canceltask))
            {
                var any = await Task.WhenAny(@this, canceltask.Task);

                if (any == canceltask.Task)
                {
                    token.ThrowIfCancellationRequested();
                }
            }

            return await @this;
        }
        public static async Task WithCancellation(this Task @this, CancellationToken token, string marker)
        {
            var canceltask = new TaskCompletionSource<CanceledInfo>();

            using (token.Register(callback: t =>
            {
                var tcs = t as TaskCompletionSource<CanceledInfo>;
                tcs!.TrySetResult(new CanceledInfo(marker));
                CanceledTable.TryAdd(tcs.Task.Result.Date, tcs.Task.Result.Marker);
            }, state: canceltask))
            {
                var any = await Task.WhenAny(@this, canceltask.Task);

                if (any == canceltask.Task)
                {
                    token.ThrowIfCancellationRequested();
                }
            }
        }
    }
}