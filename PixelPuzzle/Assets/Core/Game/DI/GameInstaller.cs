using UnityEngine;
using Zenject;

namespace PixelPuzzle
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;

        public override void InstallBindings()
        {
            Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();

            GameStatesInstaller.Install(Container);
        }
    }
}