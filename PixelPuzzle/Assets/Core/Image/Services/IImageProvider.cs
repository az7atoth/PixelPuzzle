using System;

namespace PixelPuzzle
{
    public interface IImageProvider : IDisposable
	{
        public void Initialize();
		public void LoadRandomImage();
		public void LoadNextImage();
		public bool LoadImage(int id);
		public ImageData GetImageData();
	}
}
