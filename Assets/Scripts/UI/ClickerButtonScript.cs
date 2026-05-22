using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

public class ClickerButtonScript : MonoBehaviour {
    [SerializeField] private SoundSettings audioClip;
    [SerializeField] private Button clickButton;
    [SerializeField] private float pressDuration = 0.1f;
    
    [Inject] private readonly AudioSource _audioSource;
    [Inject] private readonly MoneyUpdateService _moneyUpdateService;
    
    private Image _buttonImage;
    private Color _originalColor;
    private Tween _colorTween;
    
    private void OnEnable()
    {
        PlayerResourcesService.OnMoneyAmountChanged += OnAutomaticAction;
    }
    
    private void OnDisable()
    {
        PlayerResourcesService.OnMoneyAmountChanged -= OnAutomaticAction;
        _colorTween?.Kill();
    }
    
    private void Start()
    {
        _buttonImage = clickButton.GetComponent<Image>();
        _originalColor = _buttonImage.color;
    }
    
    private void OnAutomaticAction(float value, float difference)
    {
        PlaySound();
        SimulateButtonPress();
    }
    
    private void SimulateButtonPress()
    {
        _colorTween?.Kill();
        
        var colors = clickButton.colors;
        
        _colorTween = _buttonImage
            .DOColor(colors.pressedColor, pressDuration * 0.5f)
            .OnComplete(() => {
                _buttonImage
                    .DOColor(_originalColor, pressDuration * 0.5f);
            });
    }
    
    private void PlaySound()
    {
        if(audioClip == null || audioClip.Clip == null) return;
        audioClip.Play(_audioSource);
    }
    
    public void OnButtonClick()
    {
        PlaySound();
        _moneyUpdateService.UpdateMoneyWithCurrentSettings();
    }
}