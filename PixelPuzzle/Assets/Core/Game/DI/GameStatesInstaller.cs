using Zenject;

namespace PixelPuzzle
{
    public class GameStatesInstaller : Installer<GameStatesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameStateFactory>().AsSingle();

            Container.BindFactory<MenuState, MenuState.Factory>().AsSingle();
            Container.BindFactory<RunningState, RunningState.Factory>().AsSingle();
        }
    }
}
