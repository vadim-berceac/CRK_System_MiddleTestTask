using System;
using UnityEngine;
using Zenject;

public class EnergyUpdateService : ITickable, IDisposable, IInitializable
{
    [Inject] private EnergySettings _energySettings;
    private float _lastUpdateTime;
    
    public void Initialize()
    {
        var startEnergy = Mathf.Clamp(_energySettings.StartEnergy, 0, _energySettings.MaxEnergy);
        PlayerResourcesService.SetEnergy(startEnergy);
    }
    
    public void Tick()
    {
        if (Time.time < _lastUpdateTime + _energySettings.UpdateInterval 
            || PlayerResourcesService.GetCurrentEnergy() >= _energySettings.MaxEnergy)
        {
            return;
        }
        
        _lastUpdateTime = Time.time;

        var current = PlayerResourcesService.GetCurrentEnergy();
        var newValue = Mathf.Min(current + _energySettings.EnergyUpdateAmount, _energySettings.MaxEnergy);

        PlayerResourcesService.SetEnergy(newValue);
    }


    public void Dispose()
    {
        Debug.Log("EnergyUpdateService Dispose");
    }
}
