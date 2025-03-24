using System;
using UnityEngine;
using Zenject;
using UniRx;
using System.Linq;

namespace PixelPuzzle
{
    public class ImageProvider : IImageProvider
    {
        private IImageDatabase _database;
        private ISaveLoadService _saveLoadService;
        private IDisposable _disposable;

        private int _currentImageID;
        private ImageData _currentImageData;

        [Inject]
        public void Construct(IImageDatabase database, ISaveLoadService saveLoadService)
        {
            _database = database;
            _saveLoadService = saveLoadService;
        }

        public void Initialize()
        {
            _disposable = EventBus.OnPuzzleSolved.Subscribe(_ => SaveSolvedHash());
        }

        public void LoadRandomImage()
        {
            var hashes = _database.Images.Keys.ToList();
            var newHash = Utilities.PickRandom(ref hashes);
            LoadImage(newHash);
        }

        public void LoadNextImage()
        {
            var level = DefineNextLevelId();
            LoadImage(level);
        }

        public bool LoadImage(int id)
        {
            if (_database.Images.TryGetValue(id, out var sprite))
            {
                _currentImageID = id;
                _currentImageData = sprite;
                return true;
            }
            else
            {
                Debug.LogWarning($"Image with ID {id} not exist. Loading random image.");
                LoadRandomImage();
                return false;
            }
        }

        public ImageData GetImageData()
        {
            return _currentImageData;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        private void SaveSolvedHash()
        {
            var hashes = _saveLoadService.SaveData.SolvedImagesIDs;
            if (!hashes.Contains(_currentImageID))
            {
                hashes.Add(_currentImageID);
            }

            Debug.Log($"Solved Puzzles: {_saveLoadService.SaveData.SolvedImagesIDs.Count}");

            _saveLoadService.Save();
        }

        private int DefineNextLevelId()
        {
            foreach (var item in _database.Images)
            {
                var id = item.Key;

                if (!_saveLoadService.SaveData.SolvedImagesIDs.Contains(id))
                {
                    return id;
                }
            }

            return -1;
        }
    }
}
