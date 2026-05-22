using System;
using UnityEngine;
using Zenject;

public class MoneyUpdateService: ITickable, IDisposable, IInitializable
{
    [Inject] private ClickerSettings _clickerSettings;
    private float _lastAutoUpdateTime;
    
    public void Initialize()
    {
        PlayerResourcesService.SetMoney(_clickerSettings.StartMoneyAmount);
    }
    
    public void Tick()
    {
        if (Time.time < _lastAutoUpdateTime + _clickerSettings.UpdateInterval)
        {
           return;
        }

        UpdateMoneyWithCurrentSettings();
        
        _lastAutoUpdateTime = Time.time;
    }

    public void UpdateMoneyWithCurrentSettings()
    {
        if (PlayerResourcesService.GetCurrentEnergy() < _clickerSettings.UpdateCost)
        {
            return;
        }
        
        var newMoneyValue = PlayerResourcesService.GetCurrentMoney() + _clickerSettings.MoneyPerUpdate;
        PlayerResourcesService.SetMoney(newMoneyValue);
            
        var newEnergyValue = PlayerResourcesService.GetCurrentEnergy() - _clickerSettings.UpdateCost;
        PlayerResourcesService.SetEnergy(newEnergyValue);
    }
    
    public void Dispose()
    {
        Debug.Log("MoneyUpdateService Dispose");
    }
}
