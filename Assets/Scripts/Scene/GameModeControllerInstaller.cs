using Zenject;

/// <summary>
/// Installer for GameModeController dependencies using Zenject.
/// </summary>
public class GameModeControllerInstaller : MonoInstaller
{
    /// <summary>
    /// Binds GameModeController to a new component on a new GameObject.
    /// </summary>
    public override void InstallBindings()
    {
        Container.Bind<GameModeController>().FromNewComponentOnNewGameObject().AsSingle();
    }
}