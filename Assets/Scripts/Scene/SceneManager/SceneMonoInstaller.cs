using Zenject;

public class SceneMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SceneService>().FromNewComponentOnNewGameObject().AsSingle();
    }
}