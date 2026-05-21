using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<ClickerSettings>()
            .FromScriptableObjectResource("Data/ClickerSettings")
            .AsSingle()
            .NonLazy();
    }
}