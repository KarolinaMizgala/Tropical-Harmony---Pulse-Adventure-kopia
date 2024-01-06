using Zenject;

/// <summary>
/// Installer for WebSocketClient dependency using Zenject.
/// </summary>
public class WebSocketInstaller : MonoInstaller
{
    /// <summary>
    /// Binds WebSocketClient to a new component on a new GameObject.
    /// </summary>
    public override void InstallBindings()
    {
        Container.Bind<WebSocketClient>().FromNewComponentOnNewGameObject().AsSingle();
    }
}