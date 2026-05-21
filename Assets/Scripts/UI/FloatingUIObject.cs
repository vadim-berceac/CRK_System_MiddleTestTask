using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FloatingUIObject : MonoBehaviour
{
    [SerializeField] private Image image;
    private Sequence _sequence;
    private UIFloatingObjectPool _returnTo;

    public void Initialize(UIFloatingObjectPool returnTo)
    {
        _returnTo = returnTo;
    }
 
    public void PlayAnimation(
        Vector3 startPos,
        Vector3 endPos,
        float moveDuration,
        float waitBeforeHide = 0.2f)
    {
        _sequence?.Kill();
        
        transform.localPosition = startPos;
        SetAlpha(0f);

        _sequence = DOTween.Sequence()
            .Append(image.DOFade(1f, moveDuration * 0.2f))

            .Join(transform.DOLocalMove(endPos, moveDuration))
            .Join(image.DOFade(0f, moveDuration * 0.3f)
                .SetDelay(moveDuration * 0.7f)) 

            .AppendInterval(waitBeforeHide)
            
            .OnComplete(() => _returnTo.ReturnToPool(this));
 
        _sequence.Play();
    }
 
    public void ResetState()
    {
        _sequence?.Kill();
        SetAlpha(0f);
    }
 
    private void SetAlpha(float alpha)
    {
        if (image == null) return;
        
        var color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
