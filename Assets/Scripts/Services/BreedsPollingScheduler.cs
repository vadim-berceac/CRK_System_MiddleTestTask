using System;
using Zenject;

public class BreedsPollingScheduler : IInitializable, IDisposable
{
    [Inject] private readonly BreedsVisibilityDetector _visibilityDetector;
    [Inject] private readonly DogsService _dogsService;
    [Inject] private readonly BreedsRequestQueue _requestQueue;
    
    private bool _isBreedsTabVisible;
    
    public event Action OnStartLoadingList;
    public event Action<BreedData[]> OnBreedsListLoaded;
    public event Action OnStartLoadingDetails;
    public event Action<BreedDetailsData> OnBreedDetailsLoaded;
    public event Action<string> OnLoadError;
    
    public void Initialize()
    {
        _visibilityDetector.OnTabVisibilityChanged += OnBreedsVisibilityChanged;
        _requestQueue.OnRequestDequeued += OnRequestDequeued;
        _dogsService.OnBreedsListFetched += OnBreedsListFetched;
        _dogsService.OnBreedDetailsFetched += OnBreedDetailsFetched;
        _dogsService.OnError += OnError;
    }
    
    public void Dispose()
    {
        _visibilityDetector.OnTabVisibilityChanged -= OnBreedsVisibilityChanged;
        _requestQueue.OnRequestDequeued -= OnRequestDequeued;
        _dogsService.OnBreedsListFetched -= OnBreedsListFetched;
        _dogsService.OnBreedDetailsFetched -= OnBreedDetailsFetched;
        _dogsService.OnError -= OnError;
        
        StopPolling();
    }
    
    private void OnBreedsVisibilityChanged(bool isVisible)
    {
        _isBreedsTabVisible = isVisible;
        
        if (isVisible)
        {
            StartPolling();
        }
        else
        {
            StopPolling();
        }
    }
    
    private void StartPolling()
    {
        var listRequest = new BreedRequest { Type = BreedRequestType.ListBreeds };
        _requestQueue.Enqueue(listRequest);
    }
    
    private void StopPolling()
    {
        _dogsService.CancelListRequest();
        _dogsService.CancelDetailRequest();
        _requestQueue.Clear();
    }
    
    private async void OnRequestDequeued(BreedRequest request)
    {
        if (!_isBreedsTabVisible)
        {
            return;
        }
        
        switch (request.Type)
        {
            case BreedRequestType.ListBreeds:
                OnStartLoadingList?.Invoke();
                await _dogsService.FetchBreedsList();
                break;
                
            case BreedRequestType.BreedDetails:
                OnStartLoadingDetails?.Invoke();
                await _dogsService.FetchBreedDetails(request.BreedId);
                break;
        }
    }
    
    private void OnBreedsListFetched(BreedData[] breeds)
    {
        OnBreedsListLoaded?.Invoke(breeds);
        _requestQueue.Dequeue();
    }
    
    private void OnBreedDetailsFetched(BreedDetailsData details)
    {
        OnBreedDetailsLoaded?.Invoke(details);
        _requestQueue.Dequeue();
    }
    
    private void OnError(string error)
    {
        OnLoadError?.Invoke(error);
        _requestQueue.Dequeue();
    }
    
    public void SelectBreed(string breedId)
    {
        _dogsService.CancelDetailRequest();
        _requestQueue.Clear();
    
        var detailRequest = new BreedRequest 
        { 
            Type = BreedRequestType.BreedDetails,
            BreedId = breedId
        };
        _requestQueue.Enqueue(detailRequest);
    }
}