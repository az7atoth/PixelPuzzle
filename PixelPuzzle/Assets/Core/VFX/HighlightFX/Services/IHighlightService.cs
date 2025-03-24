using UnityEngine;

namespace PixelPuzzle
{
    public interface IHighlightService
    {
        public void Initialize();
        public void Play(Block block);
        public void Play(Vector3[] positions);
    }
}
