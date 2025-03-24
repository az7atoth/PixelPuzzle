using PixelPuzzle;
using Zenject;

public class SaveLoadInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SaveLoadService>().AsSingle();
    }
}