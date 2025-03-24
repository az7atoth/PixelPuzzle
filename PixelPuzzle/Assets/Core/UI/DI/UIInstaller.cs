using UnityEngine;
using Zenject;

namespace PixelPuzzle
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private UIController _uIController;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIController>().FromInstance(_uIController).AsSingle();
        }
    }
}