using System;
using Cysharp.Threading.Tasks;
using Zenject;
using UnityEngine;

public class WeatherPollingScheduler : IInitializable, IDisposable
{
    [Inject] private readonly WeatherTabSettings _settings;
    [Inject] private readonly WeatherVisibilityDetector _visibilityDetector;
    [Inject] private readonly WeatherService _weatherService;
    [Inject] private readonly WeatherRequestQueue _requestQueue;
    
    private bool _isWeatherTabVisible;
    private UniTaskCompletionSource _pollingTask;
    
    public void Initialize()
    {
        _visibilityDetector.OnWeatherVisibilityChanged += OnWeatherVisibilityChanged;
        _requestQueue.OnRequestDequeued += OnRequestDequeued;
        _weatherService.OnWeatherFetched += OnWeatherFetched;
        _weatherService.OnWeatherError += OnWeatherError;
    }
    
    public void Dispose()
    {
        _visibilityDetector.OnWeatherVisibilityChanged -= OnWeatherVisibilityChanged;
        _requestQueue.OnRequestDequeued -= OnRequestDequeued;
        _weatherService.OnWeatherFetched -= OnWeatherFetched;
        _weatherService.OnWeatherError -= OnWeatherError;
        
        StopPolling();
    }
    
    private void OnWeatherVisibilityChanged(bool isVisible)
    {
        _isWeatherTabVisible = isVisible;
        
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
        if (_pollingTask != null)
            return;
        
        _pollingTask = new UniTaskCompletionSource();
        PollingLoop().Forget();
    }
    
    private void StopPolling()
    {
        _weatherService.CancelRequest();
        _requestQueue.Clear();
        _pollingTask?.TrySetResult();
        _pollingTask = null;
    }
    
    private async UniTaskVoid PollingLoop()
    {
        while (_isWeatherTabVisible && _pollingTask != null)
        {
            if (!_requestQueue.HasActiveRequest)
            {
                Debug.Log($"[Polling] Enqueueing request at {Time.time}");
                var request = new WeatherRequestItem();
                _requestQueue.Enqueue(request);
            }
            else
            {
                Debug.Log($"[Polling] Request already active, skipping enqueue");
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(_settings.UpdatingInterval));
        }
    }
    
    private async void OnRequestDequeued(WeatherRequestItem request)
    {
        Debug.Log($"[Polling] Request dequeued at {Time.time}, fetching weather...");
        await _weatherService.FetchWeather();
    }
    
    private void OnWeatherFetched(WeatherData data)
    {
        Debug.Log($"[Polling] Weather fetched at {Time.time}: {data.Temperature}F");
        _requestQueue.Dequeue();
    }
    
    private void OnWeatherError(string error)
    {
        Debug.LogError($"[Polling] Weather error: {error}");
        _requestQueue.Dequeue();
    }
}