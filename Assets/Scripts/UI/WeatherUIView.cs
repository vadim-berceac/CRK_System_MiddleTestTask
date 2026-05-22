using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WeatherUIView : MonoBehaviour
{
    [SerializeField] private Image weatherIcon;
    [SerializeField] private TextMeshProUGUI temperatureText;
    
    [Inject] private readonly WeatherService _weatherService;
    [Inject] private readonly IconLoader _iconLoader;
    
    private void OnEnable()
    {
        _weatherService.OnWeatherFetched += OnWeatherFetched;
        _weatherService.OnWeatherError += OnWeatherError;
        weatherIcon.gameObject.SetActive(false);
        temperatureText.gameObject.SetActive(false);
    }
    
    private void OnDisable()
    {
        _weatherService.OnWeatherFetched -= OnWeatherFetched;
        _weatherService.OnWeatherError -= OnWeatherError;
    }
    
    private async void OnWeatherFetched(WeatherData data)
    {
        Debug.Log($"[WeatherUI] Got data at {Time.time}: {data.Temperature}F");
        var iconTexture = await _iconLoader.LoadIcon(data.IconUrl);
        if (iconTexture != null)
        {
            weatherIcon.sprite = Sprite.Create(
                iconTexture,
                new Rect(0, 0, iconTexture.width, iconTexture.height),
                Vector2.one * 0.5f
            );
            weatherIcon.gameObject.SetActive(true);
        }
       
        temperatureText.text = $"Сегодня - {data.Temperature}F";
        temperatureText.gameObject.SetActive(true);
    }
    
    private void OnWeatherError(string error)
    {
        temperatureText.text = $"Ошибка: {error}";
        Debug.LogError($"Weather UI Error: {error}");
    }
}