using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace PixelPuzzle
{
    public class ImageDatabase : IImageDatabase
    {
        public IReadOnlyDictionary<int, ImageData> Images => _images;

        private ImagesConfig _config;
        private Dictionary<int, ImageData> _images;

        [Inject]
        public void Construct(ImagesConfig config)
        {
            _config = config;
        }

        public void Initialize()
        {
            _images = new();

            var sprites = Resources.LoadAll<Sprite>(_config.ImagesFolderPath);

            ImageData data;
            var spriteSet = new List<Sprite>(100);

            for (int i = 0; i < sprites.Length; i++)
            {
                spriteSet.Add(sprites[i]);

                if (spriteSet.Count == 100)
                {
                    var imageID = _images.Count;
                    data = new ImageData(imageID, spriteSet.ToArray(), _config.Width, _config.Height);
                    _images.Add(imageID, data);
                    spriteSet.Clear();
                }
            }

            Debug.Log($"Image Database: {_images.Count} images loaded");
        }
    }
}
