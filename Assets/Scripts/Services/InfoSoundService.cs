using System;
using UnityEngine;
using Zenject;

public class InfoSoundService : IInitializable, IDisposable
{
    [Inject] private readonly AudioSource _audioSource;
    [Inject] private readonly ClickerSettings _clickerSettings;
    [Inject] private readonly EnergySettings _energySettings;

    private float _lastEnergyValue;
    private float _lastMoneyValue;
    
    public void Initialize()
    {
        PlayerResourcesService.OnEnergyAmountChanged += OnEnergyAmountIncreased;
        PlayerResourcesService.OnMoneyAmountChanged += OnMoneyAmountIncreased;

        _lastEnergyValue = PlayerResourcesService.GetCurrentEnergy();
        _lastMoneyValue = PlayerResourcesService.GetCurrentMoney();
    }

    public void Dispose()
    {
        PlayerResourcesService.OnEnergyAmountChanged -= OnEnergyAmountIncreased;
        PlayerResourcesService.OnMoneyAmountChanged -= OnMoneyAmountIncreased;
    }

    private void OnMoneyAmountIncreased(float newValue, float difference)
    {
        if (_lastMoneyValue >= newValue)
        {
            _lastMoneyValue = newValue;
            return;
        }
        _lastMoneyValue = newValue;
        _clickerSettings.UpdateSound.Play(_audioSource);
    }

    private void OnEnergyAmountIncreased(float newValue, float difference)
    {
        if (_lastEnergyValue >= newValue)
        {
            _lastEnergyValue = newValue;
            return;
        }
        _lastEnergyValue = newValue;
        _energySettings.UpdateSound.Play(_audioSource);
    }
}
