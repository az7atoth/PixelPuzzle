using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace PixelPuzzle
{
    public class FieldDataProvider : IFieldDataProvider
    {
        public IReadOnlyCollection<IReadOnlyCellData> Cells => _cells;
        public List<BlockData> Blocks => _blocks;

        private FieldConfig _fieldConfig;
        private FigureProvider _figures;
        private CellData[] _cells;
        private List<BlockData> _blocks;

        [Inject]
        public void Construct(FieldConfig fieldConfig)
        {
            _fieldConfig = fieldConfig;
        }

        public void Initialize()
        {
            _figures = new FigureProvider();
            _blocks = new List<BlockData>();
            MakeField();
        }

        public void PrepareNewField()
        {
            ClearFieldData();
            MakeBlocks();
        }

        private void ClearFieldData()
        {
            _blocks.Clear();

            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i].BlockID = -1;
            }
        }

        private void MakeBlocks()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                var cell = _cells[i];
                var figure = _figures.GetRandomFigure();
                var fit = TryFit(cell, figure);

                if (fit)
                {
                    AddBlock(cell, figure);
                }
            }

            var freeCellIds = _cells.Where(x => x.BlockID < 0).Select(x => x.ID).ToList();

            for (int i = 0; i < freeCellIds.Count; i++)
            {
                var possibleFigures = Enum.GetValues(typeof(FigureTypes)).Cast<FigureTypes>().ToList();
                var cellID = freeCellIds[i];

                var cell = _cells[cellID];
                if (cell.BlockID > 0)
                {
                    continue;
                }

                while (possibleFigures.Count > 0)
                {
                    var rndFigureDefinition = Utilities.PickRandom(ref possibleFigures);
                    var figure = _figures.Get(rndFigureDefinition);

                    var fit = TryFit(cell, figure);

                    if (fit)
                    {
                        AddBlock(cell, figure);
                        break;
                    }
                }
            }
        }

        private bool TryFit(CellData cell, Vector2Int[] cellPositions)
        {
            for (int i = 0; i < cellPositions.Length; i++)
            {
                var position = cellPositions[i] + cell.Position;

                if (!CellExist(position, _fieldConfig.Width, _fieldConfig.Height))
                {
                    return false;
                }

                var blockCellID = Utilities.GetIdFromGridPosition(position, _fieldConfig.Width);
                var blockCell = _cells[blockCellID];

                if (blockCell.BlockID >= 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void AddBlock(CellData cell, Vector2Int[] cellPositions)
        {
            var blockData = new BlockData();
            var blockID = _blocks.Count;

            blockData.BlockID = blockID;
            blockData.OriginPosition = cell.Position;
            blockData.CellPositions = new Vector2Int[cellPositions.Length];

            for (int i = 0; i < cellPositions.Length; i++)
            {
                var cellPosition = cellPositions[i] + cell.Position;

                var cellID = Utilities.GetIdFromGridPosition(cellPosition, _fieldConfig.Width);
                var blockCell = _cells[cellID];

                blockCell.BlockID = blockID;
                blockData.CellPositions[i] = cellPositions[i];
            }

            _blocks.Add(blockData);
        }

        private void MakeField()
        {
            _cells = new CellData[_fieldConfig.Width * _fieldConfig.Height];

            for (int i = 0; i < _cells.Length; i++)
            {
                var cell = new CellData();

                var position = Utilities.GetGridPosition(i, _fieldConfig.Width);

                cell.ID = i;
                cell.Position = position;
                cell.BlockID = -1;

                _cells[i] = cell;
            }
        }

        private bool CellExist(Vector2Int position, int width, int height)
        {
            return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
        }
    }
}
