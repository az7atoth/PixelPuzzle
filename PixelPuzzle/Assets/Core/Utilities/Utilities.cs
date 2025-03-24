using System.Collections.Generic;
using UnityEngine;

namespace PixelPuzzle
{
    public static class Utilities
    {
        public static int GetIdFromGridPosition(Vector2Int position, int fieldWidth)
        {
            return GetIdFromGridPosition(position.x, position.y, fieldWidth);
        }

        public static int GetIdFromGridPosition(int x, int y, int fieldWidth)
        {
            return y * fieldWidth + x;
        }

        public static Vector2Int GetGridPosition(int id, int fieldWidth)
        {
            return new Vector2Int(GetColumn(id, fieldWidth), GetRow(id, fieldWidth));
        }

        public static void GetGridPosition(int id, int fieldWidth, out int x, out int y)
        {
            x = GetColumn(id, fieldWidth);
            y = GetRow(id, fieldWidth);
        }

        private static int GetRow(int id, int fieldWidth)
        {
            return id / fieldWidth;
        }

        private static int GetColumn(int id, int fieldWidth)
        {
            return id % fieldWidth;
        }

        public static int GetRowPosition(int id, int width)
        {
            var row = GetRow(id, width);
            return id - row * width;
        }

        public static int GetIdInverted(int id, int width, int height)
        {
            var row = GetRow(id, width);
            var column = GetColumn(id, width);
            var maxY = height - 1;
            return height * (maxY - row) + column;
        }

        public static T PickRandom<T>(ref List<T> target)
        {
            if (target == null || target.Count == 0)
            {
                return default;
            }

            var rndIndex = Random.Range(0, target.Count);
            var result = target[rndIndex];
            target.RemoveAt(rndIndex);
            return result;
        }
    }
}
