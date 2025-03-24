using Az7.Utils.Pool;
using UnityEngine;
using UniRx;

namespace PixelPuzzle
{
    public class HighlightService : MonoBehaviour, IHighlightService
    {
        [SerializeField] private Pool _fxPool;

        private ShapeBuilder _shapeBuilder;

        public void Initialize()
        {
            _shapeBuilder = new ShapeBuilder();
            _fxPool.MakePool();
            EventBus.OnBlockPlaced.Subscribe(block => Play(block)).AddTo(this);
        }

        public void Play(Block block)
        {
            var shapePositions = _shapeBuilder.GetShape(block.BlockData.CellPositions);

            var lrPositions = new Vector3[shapePositions.Count];

            for (int i = 0; i < lrPositions.Length; i++)
            {
                var position = new Vector3(
                    shapePositions[i].x + block.BlockData.OriginPosition.x,
                    shapePositions[i].y + block.BlockData.OriginPosition.y,
                    0f);

                lrPositions[i] = position;
            }

            Play(lrPositions);
        }

        public void Play(Vector3[] positions)
        {
            var fx = _fxPool.Take().GetComponent<HighlightFX>();
            fx.gameObject.SetActive(true);
            fx.Play(positions);
        }
    }
}
