using System.Collections.Generic;

namespace PixelPuzzle
{
    public interface IImageDatabase
    {
        /// <summary>
        /// Number is image ID
        /// </summary>
        public IReadOnlyDictionary<int, ImageData> Images { get; }
        public void Initialize();
    }
}
