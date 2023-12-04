using UnityEngine;
using Zenject;

public class SceneChangerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SceneChanger>().FromNewComponentOnNewGameObject().AsSingle();
    }
}