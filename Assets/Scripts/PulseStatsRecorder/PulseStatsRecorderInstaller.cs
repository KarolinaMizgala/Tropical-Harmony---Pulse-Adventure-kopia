using Zenject;

/// <summary>
/// Installs the bindings for the PulseStatsRecorder.
/// </summary>
public class PulseStatsRecorderInstaller : MonoInstaller
{
    /// <summary>
    /// Installs the bindings.
    /// </summary>
    public override void InstallBindings()
    {
        // Bind the PulseStatsRecorder to a new component on a new game object
        // and make it a singleton
        Container.Bind<PulseStatsRecorder>().FromNewComponentOnNewGameObject().AsSingle();
    }
}