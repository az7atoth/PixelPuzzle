using System;

namespace PixelPuzzle
{
    public interface IBlockController : IDisposable
    {
        public void Initialize();
        public void Start();
        public void RevealRandomBlock();
        public void AutoPlaceRandomBlock();
    }
}
