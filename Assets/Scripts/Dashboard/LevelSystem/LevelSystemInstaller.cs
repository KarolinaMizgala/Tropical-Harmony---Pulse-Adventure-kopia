using Zenject;

/// <summary>
/// Installs the LevelSystem as a single instance in the game.
/// </summary>
public class LevelSystemInstaller : MonoInstaller
{
    /// <summary>
    /// Binds the LevelSystem to a new component on a new GameObject.
    /// </summary>
    public override void InstallBindings()
    {
        Container.Bind<LevelSystem>().FromNewComponentOnNewGameObject().AsSingle();
    }
}