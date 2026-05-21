using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InfoController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private Slider energySlider;
    
    [Inject] private EnergySettings _energySettings;

    private void Start()
    {
        UpdateMoneyText(PlayerResourcesService.GetCurrentMoney());
        UpdateEnergyText(PlayerResourcesService.GetCurrentEnergy());
        
        PlayerResourcesService.OnEnergyChanged += UpdateEnergyText;
        PlayerResourcesService.OnMoneyChanged += UpdateMoneyText;
    }

    private void UpdateMoneyText(float newValue)
    {
        moneyText.text = newValue.ToString();
    }

    private void UpdateEnergyText(float newValue)
    {
        energyText.text = newValue.ToString();
        energySlider.value = newValue/_energySettings.MaxEnergy;
    }

    private void OnDestroy()
    {
        PlayerResourcesService.OnEnergyChanged -= UpdateEnergyText;
        PlayerResourcesService.OnMoneyChanged -= UpdateMoneyText;
    }
}
