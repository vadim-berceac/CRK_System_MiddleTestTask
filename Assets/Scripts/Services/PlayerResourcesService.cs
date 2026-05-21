using System;

public static class PlayerResourcesService
{
    private static float _currentEnergy = 0;

    private static float _currentMoney = 0;

    public static event Action<float> OnMoneyChanged;
    public static event Action<float> OnEnergyChanged;

    public static void SetEnergy(float energy)
    {
        _currentEnergy = energy;
        OnEnergyChanged?.Invoke(_currentEnergy);
    }

    public static void SetMoney(float money)
    {
        _currentMoney = money;
        OnMoneyChanged?.Invoke(_currentMoney);
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
