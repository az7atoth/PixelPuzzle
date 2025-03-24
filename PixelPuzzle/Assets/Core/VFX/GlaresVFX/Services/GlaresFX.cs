using Az7.Utils.Pool;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UniRx;

namespace PixelPuzzle
{
    public class GlaresFX : MonoBehaviour, IGlaresFX
    {
        [SerializeField] private Transform[] _positions;
        [SerializeField] private Pool _glaresPool;
        [SerializeField] private float _glaresDelay;
        [SerializeField] private float _sizeMin = .5f;
        [SerializeField] private float _sizeMax = .8f;

        public void Initialize()
        {
            _glaresPool.MakePool();
            EventBus.OnPuzzleSolved.Subscribe(_ => Play()).AddTo(this);
        }

        public void Play()
        {
            PlayAsync(destroyCancellationToken).Forget();
        }

        private async UniTaskVoid PlayAsync(CancellationToken token)
        {
            Blackboard.SomeAnimationInProgress = true;

            token.Register(() => { return; });

            for (int i = 0; i < _positions.Length; i++)
            {
                var glare = _glaresPool.Take().GetComponent<Glare>();

                var size = Random.Range(_sizeMin, _sizeMax);

                glare.transform.position = _positions[i].position;
                glare.transform.localScale = Vector3.one * size;
                glare.gameObject.SetActive(true);

                await UniTask.WaitForSeconds(_glaresDelay, cancellationToken: token);
            }

            Blackboard.SomeAnimationInProgress = false;
        }
    }
}
