using System;

using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.Infrastructure.Disposable
{
    public class DisposableSource : IDisposable
    {
        private Action _dispose;

        public DisposableSource(Action dispose, Action now = null)
        {
            _dispose = dispose.VerifyReferenceAndSet(nameof(dispose));

            if (now != null)
            {
                now.Invoke();
            }
        }

        public IDisposable ActionNow(Action now)
        {
            now.VerifyReferenceAndSet(nameof(now)).Invoke();

            return this;
        }
        public void Dispose()
        {
            _dispose.Invoke();
        }
    }
}