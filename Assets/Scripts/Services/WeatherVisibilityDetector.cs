using System;
using Zenject;

public class WeatherVisibilityDetector : IInitializable, IDisposable
{
    [Inject] private readonly TabController _tabController;
    
    public event Action<bool> OnWeatherVisibilityChanged;
    
    public void Initialize()
    {
        _tabController.OnTabSelected += OnTabSelected;
    }

    public void Dispose()
    {
        _tabController.OnTabSelected -= OnTabSelected;
    }

    private void OnTabSelected(TabTypeEnum tabType)
    {
        if (tabType != TabTypeEnum.Weather)
        {
            OnWeatherVisibilityChanged?.Invoke(false);
            return;
        }
        OnWeatherVisibilityChanged?.Invoke(true);
    }
}
