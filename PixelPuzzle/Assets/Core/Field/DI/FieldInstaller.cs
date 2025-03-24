using PixelPuzzle;
using UnityEngine;
using Zenject;

public class FieldInstaller : MonoInstaller
{
    [SerializeField] private FieldConfig _fieldConfig;
    [SerializeField] private FieldView _fieldView;

    public override void InstallBindings()
    {
        Container.Bind<FieldConfig>().FromInstance(_fieldConfig).AsSingle();

        Container.BindInterfacesAndSelfTo<FieldDataProvider>().AsSingle();
        Container.BindInterfacesAndSelfTo<FieldView>().FromInstance(_fieldView).AsSingle();
    }
}