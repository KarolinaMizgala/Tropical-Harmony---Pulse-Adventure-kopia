using Zenject;

public class PlayerPositionManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerPositionManager>().FromNewComponentOnNewGameObject().AsSingle();
    }
}