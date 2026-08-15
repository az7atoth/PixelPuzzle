using Zenject;

namespace PixelPuzzle
{
    public class AnalyticsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AnalyticsServiceController>().AsSingle();
        }
    }
}