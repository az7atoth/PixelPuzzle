using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Az7.Utils.Pool
{
    public class Pool : MonoBehaviour
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [SerializeField] private int _poolSize;
        [SerializeField] private bool _initOnStart = true;

        private bool _isInitialized;
        private List<GameObject> _pool = new();
        private List<GameObject> _inUse = new();
        private DiContainer _container;
        private bool _prefabIsPoolable;

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _container = diContainer;
        }

        public GameObject Take()
        {
            if (_pool.Count == 0) MakeInstance(_prefabIsPoolable);

            var instance = _pool[0];
            _pool.RemoveAt(0);
            _inUse.Add(instance);

            return instance;
        }

        public bool Return(GameObject instance)
        {
            if (_inUse.Count == 0 || !_inUse.Contains(instance))
            {
                Debug.LogError("Trying return wrong GameObject");
                return false;
            }

            _inUse.Remove(instance);
            _pool.Add(instance);

            if (instance.transform.parent != transform)
            { instance.transform.SetParent(transform); }

            instance.transform.position = transform.position;
            instance.SetActive(false);

            return true;
        }

        private void Start()
        {
            if (_initOnStart)
            {
                MakePool();
            }
        }

        public void MakePool()
        {
            if (_isInitialized) return;

            _prefabIsPoolable = DefineIsPoolable();

            for (int i = 0; i < _poolSize; i++)
            {
                MakeInstance(_prefabIsPoolable);
            }

            _isInitialized = true;
        }

        private void MakeInstance(bool isPoolable)
        {
            var instance = _container.InstantiatePrefab(Prefab, transform.position,
                Quaternion.identity, transform);

            if (isPoolable)
            {
                instance.gameObject.GetComponent<PoolableObject>().Setup(this);
            }

            instance.SetActive(false);

            _pool.Add(instance);
        }

        private bool DefineIsPoolable()
        {
            return Prefab.TryGetComponent(out PoolableObject _);
        }
    }
}
