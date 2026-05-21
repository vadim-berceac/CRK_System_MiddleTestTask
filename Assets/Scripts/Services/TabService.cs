
using System;

public static class TabService
{
    private static TabTypeEnum _currentTabType = TabTypeEnum.None;
    
    public static event Action<TabTypeEnum> OnTabTypeChanged;

    public static void SetTabType(TabTypeEnum tabType)
    {
        if (_currentTabType == tabType)
        {
            _currentTabType = TabTypeEnum.None;
            OnTabTypeChanged?.Invoke(_currentTabType);
            return;
        }
        
        _currentTabType = tabType;
        OnTabTypeChanged?.Invoke(tabType);
    }

    public static TabTypeEnum GetTabType()
    {
        return _currentTabType;
    }
}
