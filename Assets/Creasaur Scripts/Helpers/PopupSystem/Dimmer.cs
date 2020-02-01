using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Dimmer : MonoBehaviour
{

    private Tween _fadeTween;
    
    public void Fade(float value)
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
        }

        if (value != 0 && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        _fadeTween = GetComponent<Image>().DOFade(value, 0.2f);
        if (value == 0)
        {
            _fadeTween.OnComplete(() => gameObject.SetActive(false));
        }
    }
}
