using UnityEngine;

namespace PixelPuzzle
{
    public class ImageData
    {
        public int ID { get; private set; }
        private Sprite[] _parts;
        private int _width;
        private int _height;

        public ImageData(int id, Sprite[] sprites, int width, int height)
        {
            ID = id;
            _parts = sprites;
            _width = width;
            _height = height;
        }

        public Sprite GetPart(int index)
        {
            var inverted = Utilities.GetIdInverted(index, _width, _height);

            if (inverted < 0 || inverted >= _parts.Length) return null;

            return _parts[inverted];
        }

        public Sprite GetPart(Vector2Int position)
        {
            var id = Utilities.GetIdFromGridPosition(position, _width);
            return GetPart(id);
        }
    }
}
