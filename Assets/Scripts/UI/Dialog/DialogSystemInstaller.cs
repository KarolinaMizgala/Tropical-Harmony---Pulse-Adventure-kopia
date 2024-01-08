using Zenject;

/// <summary>
/// Installer for DialogSystem and DialogWindow dependencies using Zenject.
/// </summary>
public class DialogSystemInstaller : MonoInstaller
{
    /// <summary>
    /// Binds DialogSystem and DialogWindow to their respective components.
    /// DialogSystem is bound to a new component on the current GameObject.
    /// DialogWindow is loaded from a resource.
    /// </summary>
    public override void InstallBindings()
    {
        Container.Bind<DialogSystem>().FromNewComponentOn(gameObject).AsSingle().NonLazy();
        Container.Bind<DialogWindow>().FromResource("Prefab/DialogWindow");
    }
}