using PixelPuzzle;
using UnityEngine;
using Zenject;

public class HighlightsInstaller : MonoInstaller
{
    [SerializeField] private HighlightService _highlightService;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<HighlightService>().FromInstance(_highlightService).AsSingle();
    }
}