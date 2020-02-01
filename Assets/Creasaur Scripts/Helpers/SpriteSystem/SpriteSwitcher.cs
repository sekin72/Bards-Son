using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcher : MonoBehaviour
{
    [SerializeField] private Image _pressedImage;
    [SerializeField] private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    private void SwitchSprite()
    {

    }
}
