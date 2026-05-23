using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BreedsUIView : MonoBehaviour
{
    [SerializeField] private Transform breedsListContainer;
    [SerializeField] private GameObject breedButtonPrefab;
    [SerializeField] private ScrollRect breedsScrollRect;
    [SerializeField] private int results = 10;
    
    [Inject] private readonly BreedsPollingScheduler _scheduler;
    [Inject] private readonly LoadingIndicator _loadingIndicator;
    
    private BreedData[] _currentBreeds;
    
    private void OnEnable()
    {
        _scheduler.OnStartLoadingList += OnStartLoadingList;
        _scheduler.OnBreedsListLoaded += OnBreedsListLoaded;
        _scheduler.OnLoadError += OnLoadError;
    }
    
    private void OnDisable()
    {
        _scheduler.OnStartLoadingList -= OnStartLoadingList;
        _scheduler.OnBreedsListLoaded -= OnBreedsListLoaded;
        _scheduler.OnLoadError -= OnLoadError;
    }
    
    private void OnStartLoadingList()
    {
        _loadingIndicator.Show("Загружаем породы...");
        ClearList();
    }
    
    private void OnBreedsListLoaded(BreedData[] breeds)
    {
        Debug.Log($"[BreedsUIView] Breeds loaded: {breeds.Length}");
        _loadingIndicator.Hide();
        
        _currentBreeds = breeds;
        DisplayBreeds(breeds);
    }
    
    private void OnLoadError(string error)
    {
        _loadingIndicator.Hide();
    }
    
    private void DisplayBreeds(BreedData[] breeds)
    {
        ClearList();
        
        var count = Mathf.Min(breeds.Length, results);
        
        for (var i = 0; i < count; i++)
        {
            var breed = breeds[i];
            var buttonGo = Instantiate(breedButtonPrefab, breedsListContainer);
            var button = buttonGo.GetComponent<Button>();
            var text = buttonGo.GetComponentInChildren<TextMeshProUGUI>();
            
            text.text = $"{i + 1} - {breed.Name}";
        
            var breedId = breed.Id;
            button.onClick.AddListener(() => OnBreedSelected(breedId));
        }
    }
    
    private void OnBreedSelected(string breedId)
    {
        _scheduler.SelectBreed(breedId);
    }
    
    private void ClearList()
    {
        foreach (Transform child in breedsListContainer)
        {
            Destroy(child.gameObject);
        }
    }
}