using System;
using System.Linq;
using UnityEngine;

public class TabController : MonoBehaviour
{
    [SerializeField] private TabData[] tabs;
    
    private TabData _currentTab;
    
    public event Action<TabTypeEnum> OnTabSelected;

    private void Start()
    {
        HideAll();
        TabService.OnTabTypeChanged += OnTabButtonClick;
    }

    private void OnTabButtonClick(TabTypeEnum tabType)
    {
        switch (tabType)
        {
            case TabTypeEnum.None:
                SetNewTab(TabTypeEnum.None);
                HideAll();
                break;
            
            case TabTypeEnum.Clicker:
                SetNewTab(TabTypeEnum.Clicker);
                break;
            
            case TabTypeEnum.Weather:
                SetNewTab(TabTypeEnum.Weather);
                break;
            
            case TabTypeEnum.Breeds:
                SetNewTab(TabTypeEnum.Breeds);
                break;
            
            default:
                HideAll();
                break;
        }
    }

    private void SetNewTab(TabTypeEnum tabType)
    {
        _currentTab?.Window.SetActive(false);
        _currentTab = tabs.FirstOrDefault(t => t.TabType == tabType);
        _currentTab?.Window.SetActive(true);
        OnTabSelected?.Invoke(tabType);
    }

    private void HideAll()
    {
        foreach (var tab in tabs)
        {
            tab.Window.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        TabService.OnTabTypeChanged -= OnTabButtonClick;
    }
}

[System.Serializable]
public class TabData
{
    [field: SerializeField] public GameObject Window { get; set; }
    [field: SerializeField] public TabTypeEnum TabType { get; set; }
}
