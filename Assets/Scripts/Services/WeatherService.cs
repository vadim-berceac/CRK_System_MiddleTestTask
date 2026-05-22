using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class WeatherService
{
    [Inject] private readonly WeatherTabSettings _settings;
    
    public event Action<WeatherData> OnWeatherFetched;
    public event Action<string> OnWeatherError;
    
    private CancellationTokenSource _currentRequestCts;
    
    public async UniTask FetchWeather(CancellationToken externalToken = default)
    {
        Debug.Log($"[WeatherService] Starting fetch at {Time.time}");
        _currentRequestCts?.Cancel();
        _currentRequestCts?.Dispose();
        
        _currentRequestCts = new CancellationTokenSource();
       
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
            externalToken, 
            _currentRequestCts.Token
        );
        
        try
        {
            using var request = UnityWebRequest.Get(_settings.Address);
            var asyncOp = request.SendWebRequest();
            
            await asyncOp.WithCancellation(linkedCts.Token);
            
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[WeatherService] Fetch failed: {request.error}");
                OnWeatherError?.Invoke(request.error);
                return;
            }
            
            var json = request.downloadHandler.text;
            var weatherData = ParseWeatherResponse(json);
            
            Debug.Log($"[WeatherService] Fetch success at {Time.time}: {weatherData.Temperature}F");
            OnWeatherFetched?.Invoke(weatherData);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Weather request cancelled");
        }
        catch (Exception ex)
        {
            OnWeatherError?.Invoke(ex.Message);
        }
    }
    
    public void CancelRequest()
    {
        _currentRequestCts?.Cancel();
    }
    
    private WeatherData ParseWeatherResponse(string json)
    {
        var parsed = JsonUtility.FromJson<WeatherApiResponse>(json);
        
        if (parsed?.properties?.periods != null && parsed.properties.periods.Length > 0)
        {
            var period = parsed.properties.periods[0];
            return new WeatherData
            {
                Temperature = period.temperature,
                IconUrl = period.icon,
                Forecast = period.shortForecast
            };
        }
        
        throw new Exception("Invalid weather response");
    }
}

public class WeatherData
{
    public int Temperature { get; set; }
    public string IconUrl { get; set; }
    public string Forecast { get; set; }
}

[System.Serializable]
public class WeatherApiResponse
{
    public Properties properties;
}

[System.Serializable]
public class Properties
{
    public Period[] periods;
}

[System.Serializable]
public class Period
{
    public int temperature;
    public string icon;
    public string shortForecast;
}