using UnityEngine;
using Zenject;

public class LevelSystemInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<LevelSystem>().FromNewComponentOnNewGameObject().AsSingle();
    }
}