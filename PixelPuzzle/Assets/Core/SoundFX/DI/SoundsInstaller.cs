using UnityEngine;
using Zenject;

namespace PixelPuzzle
{
    public class SoundsInstaller : MonoInstaller
    {
        [SerializeField] private SFX_Config _SFX_Config;
        [SerializeField] private SoundController _soundController;

        public override void InstallBindings()
        {
            Container.Bind<SFX_Config>().FromInstance(_SFX_Config).AsSingle();
            Container.BindInterfacesAndSelfTo<SoundController>().FromInstance(_soundController).AsSingle();
        }
    }
}