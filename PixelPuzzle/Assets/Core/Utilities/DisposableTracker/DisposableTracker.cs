using System;
using System.Collections.Generic;

namespace Az7.Utils.Disposables
{
    public class DisposableTracker : IDisposable
    {
        private List<IDisposable> _disposables;
        private bool _isDisposed;

        public bool IsDisposed => _isDisposed;

        public int DisposablesCount => _disposables != null ? _disposables.Count : 0;

        public DisposableTracker()
        {
            _disposables = new List<IDisposable>();
        }

        public void Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public void Dispose()
        {
            _disposables.ForEach(e => e.Dispose());
            _disposables.Clear();

            _isDisposed = true;
        }
    }
}
