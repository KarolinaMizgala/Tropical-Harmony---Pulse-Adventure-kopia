using Zenject;

/// <summary>
/// Class responsible for installing bindings for the scene service in the dependency injection container.
/// </summary>
public class SceneMonoInstaller : MonoInstaller
{
    /// <summary>
    /// Installs bindings for the scene service in the dependency injection container.
    /// </summary>
    public override void InstallBindings()
    {
        Container.Bind<SceneService>().FromNewComponentOnNewGameObject().AsSingle();
    }
}