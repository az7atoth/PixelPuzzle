using Az7.Utils.Pool;
using UnityEngine;
using UniRx;

namespace PixelPuzzle
{
    public class Glare : MonoBehaviour
    {
        [SerializeField] private PoolableObject _poolableObject;
        [SerializeField] private SimpleSpriteAnimator _spriteAnimator;

        private void Awake()
        {
            _spriteAnimator.OnAnimationStopped.Subscribe(_ => _poolableObject.Return()).AddTo(this);
        }
    }
}
