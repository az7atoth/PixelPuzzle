using UnityEngine;
using Zenject;

namespace PixelPuzzle
{
    public class ImageInstaller : MonoInstaller
    {
        [SerializeField] private ImagesConfig _imagesConfig;

        public override void InstallBindings()
        {
            Container.Bind<ImagesConfig>().FromInstance(_imagesConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<ImageProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<ImageDatabase>().AsSingle();
        }
    }
}