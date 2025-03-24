using UnityEngine;
using Zenject;

namespace PixelPuzzle
{
    public class GlaresInstaller : MonoInstaller
    {
        [SerializeField] private GlaresFX _glaresFX;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GlaresFX>().FromInstance(_glaresFX).AsSingle();
        }
    }
}