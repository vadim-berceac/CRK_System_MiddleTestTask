using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIndicator : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Image spinnerImage;
    [SerializeField] private float spinSpeed = 360f;
    private const string TextToDisplay = "Загружаем...";
    
    private void Start()
    {
        loadingPanel.SetActive(false);
    }
    
    private void Update()
    {
        if (loadingPanel.activeInHierarchy && spinnerImage != null)
        {
            spinnerImage.transform.Rotate(0, 0, -spinSpeed * Time.deltaTime);
        }
    }
    
    public void Show(string message = TextToDisplay)
    {
        loadingPanel.SetActive(true);
        if (loadingText != null)
        {
            loadingText.text = message;
        }
    }
    
    public void Hide()
    {
        loadingPanel.SetActive(false);
    }
}