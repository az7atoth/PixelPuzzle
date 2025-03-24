using UnityEngine;

namespace PixelPuzzle
{
    public class CellData : IReadOnlyCellData
    {
        public int ID { get; set; }
        public int BlockID { get; set; }
        public Vector2Int Position { get; set; }
    }

    public interface IReadOnlyCellData
    {
        public int ID { get; }
        public int BlockID { get; }
        public Vector2Int Position { get; }
    }
}
