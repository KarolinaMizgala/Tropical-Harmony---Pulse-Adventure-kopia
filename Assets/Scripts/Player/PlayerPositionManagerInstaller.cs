using Zenject;

/// <summary>
/// Installs the bindings for the PlayerPositionManager.
/// </summary>
public class PlayerPositionManagerInstaller : MonoInstaller
{
    /// <summary>
    /// Installs the bindings.
    /// </summary>
    public override void InstallBindings()
    {
        // Bind the PlayerPositionManager to a new component on a new game object
        // and make it a singleton
        Container.Bind<PlayerPositionManager>().FromNewComponentOnNewGameObject().AsSingle();
    }
}