using System.Collections.Generic;

namespace PixelPuzzle
{
    public interface IBlockSpawner
    {
        public int MaxSpawnCount { get; }
        public IReadOnlyList<Block> SpawnedBlocks { get; }
        public void Initialize();
        public void ReturnAll();
        public Block Spawn(BlockData blockData);
    }
}
