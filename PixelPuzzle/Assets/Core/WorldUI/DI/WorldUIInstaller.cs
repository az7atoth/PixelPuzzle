using UnityEngine;
using Zenject;

namespace PixelPuzzle
{
    public class WorldUIInstaller : MonoInstaller
    {
        [SerializeField] private WorldUI _worldUI;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WorldUI>().FromInstance(_worldUI).AsSingle();
        }
    }
}