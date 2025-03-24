namespace PixelPuzzle
{
    public interface ISoundController
    {
        public bool IsMuted { get; }
        public void Initialize();
        public void Play(SoundKey key, float pitch = 1f);
        public void SetVolume(float volume);
    }
}


