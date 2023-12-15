using UnityEngine;
using Zenject;

public class DialogSystemInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<DialogSystem>().FromNewComponentOn(gameObject).AsSingle().NonLazy();
        Container.Bind<DialogWindow>().FromResource("Prefab/DialogWindow");

    }
}