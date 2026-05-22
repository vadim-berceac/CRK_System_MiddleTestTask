using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject audioSourcePrefab;
    public override void InstallBindings()
    {
        Container
            .Bind<AudioSource>()
            .FromComponentInNewPrefab(audioSourcePrefab)
            .AsSingle()
            .NonLazy();
        
        Container
            .Bind<Canvas>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();
        
        Container
            .Bind<TabController>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();
        
        Container
            .Bind<ClickerSettings>()
            .FromScriptableObjectResource("Data/ClickerSettings")
            .AsSingle()
            .NonLazy();
        
        Container
            .Bind<EnergySettings>()
            .FromScriptableObjectResource("Data/EnergySettings")
            .AsSingle()
            .NonLazy();
        
        Container
            .Bind<WeatherTabSettings>()
            .FromScriptableObjectResource("Data/WeatherTabSettings")
            .AsSingle()
            .NonLazy();
        
        Container
            .Bind<BreedTabSettings>()
            .FromScriptableObjectResource("Data/BreedTabSettings")
            .AsSingle()
            .NonLazy();

        Container
            .Bind<PoolContainer>()
            .FromScriptableObjectResource("Data/PoolContainer")
            .AsSingle()
            .NonLazy();
        
        Container
            .BindInterfacesAndSelfTo<EnergyUpdateService>()
            .AsSingle()
            .NonLazy();
        
        Container
            .BindInterfacesAndSelfTo<MoneyUpdateService>()
            .AsSingle()
            .NonLazy();
        
        Container
            .BindInterfacesAndSelfTo<InfoSoundService>()
            .AsSingle()
            .NonLazy();
        
        Container
            .BindInterfacesAndSelfTo<WeatherVisibilityDetector>()
            .AsSingle()
            .NonLazy();
        
        Container
            .BindInterfacesAndSelfTo<WeatherPollingScheduler>()
            .AsSingle()
            .NonLazy();
        
        Container
            .BindInterfacesAndSelfTo<WeatherService>()
            .AsSingle()
            .NonLazy();
        
        Container
            .BindInterfacesAndSelfTo<WeatherRequestQueue>()
            .AsSingle()
            .NonLazy();
        
        Container
            .BindInterfacesAndSelfTo<IconLoader>()
            .AsSingle()
            .NonLazy();
    }
}