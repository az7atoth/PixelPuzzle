using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static PixelPuzzle.SFX_Config;

namespace PixelPuzzle
{
    public class SoundController : MonoBehaviour, ISoundController
    {
        public bool IsMuted => _volume == 0f;

        [SerializeField] private AudioSource _audioSourcePrefab;
        [SerializeField] private List<AudioSource> _audioSources;

        private float _volume = 1f;

        private SFX_Config _config;
        private Dictionary<SoundKey, SFX_Setting> _sfxSettins;

        [Inject]
        public void Construct(SFX_Config config)
        {
            _config = config;
        }

        public void Initialize()
        {
            _sfxSettins = new();

            foreach (var item in _config.Settings)
            {
                _sfxSettins.Add(item.Key, item);
            }
        }

        public void Play(SoundKey key, float pitch = 1f)
        {
            if (_volume == 0f) { return; }

            if (!_sfxSettins.TryGetValue(key, out var setting))
            {
                return;
            }

            var audioSource = GetFreeSource();

            audioSource.pitch = pitch;

            var rndIndex = Random.Range(0, setting.AudioClips.Length);
            var clip = setting.AudioClips[rndIndex];
            audioSource.PlayOneShot(clip, _volume);
        }

        public void SetVolume(float volume)
        {
            _volume = volume;
        }

        private AudioSource GetFreeSource()
        {
            for (int i = 0; i < _audioSources.Count; i++)
            {
                if (!_audioSources[i].isPlaying)
                {
                    return _audioSources[i];
                }
            }

            var newSource = Instantiate(_audioSourcePrefab, transform);
            _audioSources.Add(newSource);
            return newSource;
        }
    }
}


