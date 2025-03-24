using System;

namespace Az7.Utils.Disposables
{
    public static class DisposableExtensions
    {
        public static void AddTo(this IDisposable disposable, DisposableTracker tracker)
        {
            if (tracker != null)
            {
                tracker.Add(disposable);
            }
        }
    }
}
