using System;

public static class PlayerResourcesService
{
    private static float _currentEnergy = 0;

    private static float _currentMoney = 0;

    public static event Action<float, float> OnMoneyAmountChanged;
    public static event Action<float, float> OnEnergyAmountChanged;

    public static void SetEnergy(float energy)
    {
        var difference = _currentEnergy - energy;
        _currentEnergy = energy;
        OnEnergyAmountChanged?.Invoke(_currentEnergy, difference);
    }

    public static void SetMoney(float money)
    {
        var difference = _currentMoney - money;
        _currentMoney = money;
        OnMoneyAmountChanged?.Invoke(_currentMoney, difference);
    }

    public static float GetCurrentEnergy()
    {
        return _currentEnergy;
    }

    public static float GetCurrentMoney()
    {
        return _currentMoney;
    }
}
