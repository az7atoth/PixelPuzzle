using UnityEngine;
using Zenject;

namespace PixelPuzzle
{
    public class CurtainInstaller : MonoInstaller
    {
        [SerializeField] private Curtain _curtain;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Curtain>().FromInstance(_curtain).AsSingle();

        }
    }
}