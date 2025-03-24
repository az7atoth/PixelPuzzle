using UnityEngine;

namespace Az7.Utils.Pool
{
    public class PoolableObject : MonoBehaviour
    {
        public bool ReturnRequested { get; private set; }
        private Pool _pool;

        public void Setup(Pool pool)
        {
            _pool = pool;
        }

        public void Return()
        {
            if (_pool != null && !ReturnRequested)
            {
                ReturnRequested = true;
                if (!_pool.Return(this.gameObject))
                {
                    Debug.LogWarning($"Poolable {gameObject.name} failed to return. Destroyed");
                    Destroy(this.gameObject);
                }
            }
            else
            {
                var reason = ReturnRequested ? "Already requested." : "Pull is null.";
                Debug.LogWarning($"Poolable {gameObject.name} failed to return. {reason}");
            }

        }

        private void OnEnable()
        {
            ReturnRequested = false;
        }
    }
}
