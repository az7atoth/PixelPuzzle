using System;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace PixelPuzzle
{
    public class BlockController : IBlockController
    {
        private IBlockSpawner _blockSpawner;
        private IFieldDataProvider _fieldDataProvider;
        private IFieldView _fieldView;

        private IDisposable _disposable;

        [Inject]
        public void Construct(
            IBlockSpawner blockSpawner,
            IFieldDataProvider fieldDataProvider,
            IFieldView fieldView
            )
        {
            _blockSpawner = blockSpawner;
            _fieldDataProvider = fieldDataProvider;
            _fieldView = fieldView;
        }

        public void Initialize()
        {
            _disposable = EventBus.OnBlockPlaced.Subscribe(block => OnBlockPlaced(block));
        }

        public void Start()
        {
            _blockSpawner.ReturnAll();

            while (_blockSpawner.SpawnedBlocks.Count < _blockSpawner.MaxSpawnCount)
            {
                if (_fieldDataProvider.Blocks.Count == 0) break;
                SpawnNewBlock();
            }
        }

        public void AutoPlaceRandomBlock()
        {
            if (Blackboard.SomeAnimationInProgress || _blockSpawner.SpawnedBlocks.Count == 0) return;

            var blocks = _blockSpawner.SpawnedBlocks;
            var rndIndex = Random.Range(0, blocks.Count);
            blocks[rndIndex].AutoPlace();
        }

        public void RevealRandomBlock()
        {
            var blockData = PickRandomBlockData();
            if (blockData == null) return;
            RevealBlock(blockData);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        private void RevealBlock(BlockData blockData)
        {
            for (int i = 0; i < blockData.CellPositions.Length; i++)
            {
                var position = blockData.CellPositions[i] + blockData.OriginPosition;
                var cellID = Utilities.GetIdFromGridPosition(position, 10);
                _fieldView.OpenCell(cellID);
            }

            if (_fieldDataProvider.Blocks.Count == 0 && _blockSpawner.SpawnedBlocks.Count == 0)
            {
                EventBus.OnPuzzleSolved.Execute();
            }
        }

        private void OnBlockPlaced(Block block)
        {
            block.Return();
            RevealBlock(block.BlockData);

            if (!Blackboard.GameInProgress) return;

            SpawnNewBlock();
        }

        private BlockData PickRandomBlockData()
        {
            var blocks = _fieldDataProvider.Blocks;
            return Utilities.PickRandom(ref blocks);
        }

        private void SpawnNewBlock()
        {
            var blockData = PickRandomBlockData();

            if (blockData == default)
            {
                return;
            }

            _blockSpawner.Spawn(blockData);
        }

    }
}
