using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelPuzzle
{
    public class ShapeBuilder
    {
        private List<Vector2Int> _exceptions;
        private List<Vector2Int> _shape;
        private List<Vector2Int> _points;

        public ShapeBuilder()
        {
            _exceptions = new List<Vector2Int>();
            _shape = new List<Vector2Int>();
            _points = new List<Vector2Int>();
        }

        public List<Vector2Int> GetShape(Vector2Int[] cellOrigins)
        {
            GetPoints(cellOrigins);
            GetExceptions();
            BuildShape();
            return _shape;
        }

        private void GetPoints(Vector2Int[] cellOrigins)
        {
            _points.Clear();

            for (int i = 0; i < cellOrigins.Length; i++)
            {
                var position = cellOrigins[i];
                var up = position + Vector2Int.up;
                var right = position + Vector2Int.right;
                var upRight = position + Vector2Int.up + Vector2Int.right;

                if (!_points.Contains(position)) { _points.Add(position); }
                if (!_points.Contains(up)) { _points.Add(up); }
                if (!_points.Contains(right)) { _points.Add(right); }
                if (!_points.Contains(upRight)) { _points.Add(upRight); }
            }
        }

        private void GetExceptions()
        {
            _exceptions.Clear();

            int neighbours;

            for (int i = 0; i < _points.Count; i++)
            {
                neighbours = 0;

                var position = _points[i];

                if (_points.Contains(position + Vector2Int.left + Vector2Int.up)) neighbours++;     //TOP LEFT
                if (_points.Contains(position + Vector2Int.up)) neighbours++;                       //TOP MIDDLE
                if (_points.Contains(position + Vector2Int.right + Vector2Int.up)) neighbours++;    //TOP RIGHT

                if (_points.Contains(position + Vector2Int.left)) neighbours++;                     //MIDDLE LEFT
                if (_points.Contains(position + Vector2Int.right)) neighbours++;                    //MIDDLE RIGHT

                if (_points.Contains(position + Vector2Int.left + Vector2Int.down)) neighbours++;   //BOTTOM LEFT
                if (_points.Contains(position + Vector2Int.down)) neighbours++;                     //BOTTOM MIDDLE
                if (_points.Contains(position + Vector2Int.right + Vector2Int.down)) neighbours++;  //BOTTOM RIGHT

                if (neighbours >= 8)
                {
                    _exceptions.Add(position);
                }
            }
        }

        private void BuildShape()
        {
            _shape.Clear();

            var startPosition = FindStartPosition();

            Vector2Int currentPosition = startPosition;
            Vector2Int nextPosition;

            do
            {
                nextPosition = FindNextPosition(currentPosition);

                if (nextPosition == currentPosition)
                {
                    _shape.Add(nextPosition);
                    break;
                }

                _shape.Add(currentPosition);
                currentPosition = nextPosition;

            } while (true);
        }

        private Vector2Int FindStartPosition()
        {
            var result = new Vector2Int(4, 0);

            for (int i = 0; i < _points.Count; i++)
            {
                var point = _points[i];

                if (point.x < result.x && point.y == 0)
                {
                    result = point;
                }
            }

            return result;
        }

        private bool ValidatePosition(Vector2Int position)
        {
            return _points.Contains(position)
                && !_exceptions.Contains(position)
                && !_shape.Contains(position);
        }

        private Vector2Int FindNextPosition(Vector2Int position)
        {
            var left = position + Vector2Int.left;
            var right = position + Vector2Int.right;
            var up = position + Vector2Int.up;
            var down = position + Vector2Int.down;

            if (ValidatePosition(left)) return left;
            if (ValidatePosition(up)) return up;
            if (ValidatePosition(right)) return right;
            if (ValidatePosition(down)) return down;

            return position;
        }
    }
}
