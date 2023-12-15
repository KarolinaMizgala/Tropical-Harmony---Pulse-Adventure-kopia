using UnityEngine;
using Zenject;

public class GameModeControllerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameModeController>().FromNewComponentOnNewGameObject().AsSingle();
    }
}