using UnityEngine;
using Zenject;

public class WebSocketInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<WebSocketClient>().FromNewComponentOnNewGameObject().AsSingle();
    }
}