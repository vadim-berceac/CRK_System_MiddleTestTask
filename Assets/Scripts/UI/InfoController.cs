using System.Globalization;
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
        UpdateMoneyAmountText(PlayerResourcesService.GetCurrentMoney(), 0);
        UpdateEnergyAmountText(PlayerResourcesService.GetCurrentEnergy(), 0);
        
        PlayerResourcesService.OnEnergyAmountChanged += UpdateEnergyAmountText;
        PlayerResourcesService.OnMoneyAmountChanged += UpdateMoneyAmountText;
    }

    private void UpdateMoneyAmountText(float newValue, float difference)
    {
        moneyText.text = newValue.ToString(CultureInfo.InvariantCulture);
    }

    private void UpdateEnergyAmountText(float newValue, float difference)
    {
        energyText.text = newValue.ToString(CultureInfo.InvariantCulture);
        energySlider.value = newValue/_energySettings.MaxEnergy;
    }

    private void OnDestroy()
    {
        PlayerResourcesService.OnEnergyAmountChanged -= UpdateEnergyAmountText;
        PlayerResourcesService.OnMoneyAmountChanged -= UpdateMoneyAmountText;
    }
}
