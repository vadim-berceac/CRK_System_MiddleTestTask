using System;
using Zenject;

public class WeatherVisibilityDetector : VisibilityDetector
{
    protected override TabTypeEnum TabType()
    {
        return TabTypeEnum.Weather;
    }
}

public abstract class VisibilityDetector : IInitializable, IDisposable
{
    [Inject] private readonly TabController _tabController;
    
    public event Action<bool> OnTabVisibilityChanged;
    
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
        if (tabType != TabType())
        {
            OnTabVisibilityChanged?.Invoke(false);
            return;
        }
        OnTabVisibilityChanged?.Invoke(true);
    }

    protected abstract TabTypeEnum TabType();
}