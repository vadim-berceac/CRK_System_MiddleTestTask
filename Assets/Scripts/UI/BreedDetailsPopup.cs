using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BreedDetailsPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI breedNameText;
    [SerializeField] private TextMeshProUGUI breedDescriptionText;
    [SerializeField] private Button closeButton;
    [SerializeField] private LayoutElement layoutElement;
    
    [Inject] private readonly BreedsPollingScheduler _scheduler;
    [Inject] private readonly LoadingIndicator _loadingIndicator;
    
    private void OnEnable()
    {
        _scheduler.OnStartLoadingDetails += OnStartLoadingDetails;
        _scheduler.OnBreedDetailsLoaded += OnBreedDetailsLoaded;
        closeButton.onClick.AddListener(ClosePopup);
    }
    
    private void OnDisable()
    {
        _scheduler.OnStartLoadingDetails -= OnStartLoadingDetails;
        _scheduler.OnBreedDetailsLoaded -= OnBreedDetailsLoaded;
        closeButton.onClick.RemoveListener(ClosePopup);
    }
    
    private void Start()
    {
        popupPanel.SetActive(false);
    }
    
    private void OnStartLoadingDetails()
    {
        _loadingIndicator.Show("Загружаем информацию о породе...");
    }
    
    private void OnBreedDetailsLoaded(BreedDetailsData details)
    {
        _loadingIndicator.Hide();
        
        DisplayBreedDetails(details);
    }
    
    private void DisplayBreedDetails(BreedDetailsData details)
    {
        breedNameText.text = details.Name ?? "Неизвестная порода";
        breedDescriptionText.text = details.Description ?? "Описание отсутствует";
    
        AdjustPopupHeight();
    
        popupPanel.SetActive(true);
    }
    
    private void AdjustPopupHeight()
    {
        layoutElement.preferredHeight = 400f;
    }
    
    private void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}