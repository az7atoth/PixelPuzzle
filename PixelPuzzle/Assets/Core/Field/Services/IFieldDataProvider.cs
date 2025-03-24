using System.Collections.Generic;

namespace PixelPuzzle
{
    public interface IFieldDataProvider
    {
        public List<BlockData> Blocks { get; }
        public IReadOnlyCollection<IReadOnlyCellData> Cells { get; }
        public void Initialize();
        public void PrepareNewField();
    }
}
