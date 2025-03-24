using Az7.Utils.Pool;
using System;
using UnityEngine;
using Zenject;
using UniRx;
using System.Collections.Generic;

namespace PixelPuzzle
{
    public class BlockSpawner : MonoBehaviour, IBlockSpawner
    {
        public int MaxSpawnCount => _spawnPoints.Length;
        public IReadOnlyList<Block> SpawnedBlocks => _spawnedBlocks;

        [SerializeField] private Pool _blockPool;
        [SerializeField] private BlockSpawnPoint[] _spawnPoints;

        private List<Block> _spawnedBlocks;
        private List<int> _spawnIndexes;
        private IDisposable _disposable;

        private IImageProvider _imageProvider;

        [Inject]
        public void Construct(IImageProvider imageProvider)
        {
            _imageProvider = imageProvider;
        }

        public void Initialize()
        {
            _spawnIndexes = new List<int>();

            for (int i = 0; i < MaxSpawnCount; i++)
            {
                _spawnIndexes.Add(i);
            }

            _spawnedBlocks = new(MaxSpawnCount);
            _blockPool.MakePool();

            SubscribeOnBlockReturned();
        }

        public void ReturnAll()
        {
            _disposable?.Dispose();

            foreach (var block in _spawnedBlocks)
            {
                _spawnIndexes.Add(block.SpawnIndex);
                block.Return();
            }

            _spawnedBlocks.Clear();

            SubscribeOnBlockReturned();
        }

        public Block Spawn(BlockData blockData)
        {
            var spawnIndex = GetFreeSpawnIndex();

            if (spawnIndex < 0)
            {
                return null;
            }

            var newBlock = _blockPool.Take().GetComponent<Block>();

            newBlock.gameObject.SetActive(true);

            newBlock.transform.position = _spawnPoints[spawnIndex].transform.position;
            newBlock.Setup(spawnIndex, _imageProvider.GetImageData(), blockData);

            _spawnedBlocks.Add(newBlock);

            _spawnPoints[spawnIndex].PlaySpawnAnimation(newBlock);

            return newBlock;
        }

        private void RemoveBlock(Block block)
        {
            _spawnIndexes.Add(block.SpawnIndex);
            _spawnedBlocks.Remove(block);
        }

        private int GetFreeSpawnIndex()
        {
            if (_spawnIndexes.Count == 0)
            {
                return -1;
            }
            else
            {
                var index = _spawnIndexes[0];
                _spawnIndexes.RemoveAt(0);
                return index;
            }
        }

        private void SubscribeOnBlockReturned()
        {
            _disposable = EventBus.OnBlockReturned.Subscribe(block => RemoveBlock(block));
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}
