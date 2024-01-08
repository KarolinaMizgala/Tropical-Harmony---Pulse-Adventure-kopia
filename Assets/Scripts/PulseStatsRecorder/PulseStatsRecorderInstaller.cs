using Zenject;

public class PulseStatsRecorderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PulseStatsRecorder>().FromNewComponentOnNewGameObject().AsSingle();
    }
}