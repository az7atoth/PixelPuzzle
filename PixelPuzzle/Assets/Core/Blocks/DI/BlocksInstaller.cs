using PixelPuzzle;
using UnityEngine;
using Zenject;

public class BlocksInstaller : MonoInstaller
{
    [SerializeField] private BlockSpawner _blockSpawner;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<BlockSpawner>().FromInstance(_blockSpawner).AsSingle();
        Container.BindInterfacesAndSelfTo<BlockController>().AsSingle();
    }
}