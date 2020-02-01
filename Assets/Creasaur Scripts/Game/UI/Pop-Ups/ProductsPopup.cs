using System;
using System.Collections;
using System.Collections.Generic;
using PopupSystem;
using UnityEngine;
using UnityEngine.UI;

public class ProductsPopup : PopupSystem.PopupBase
{
    [SerializeField] private Image[] _productImages;
    [SerializeField] private Text[] _productNames;

    public override PopupType PopupType
    {
        get
        {
            return PopupSystem.PopupType.Products;
        }
    }

    public override void Show(Action callback = null, Hashtable parameters = null)
    {
        var productSprites = (Sprite[])parameters["Sprites"];
        var lockedProductSprites = (Sprite[])parameters["LockedSprites"];
        var productNames = (string[])parameters["ProductNames"];
        var availableProductIndex = (int)parameters["ProductIndex"];

        for(int i = 0; i < productSprites.Length; i++)
        {
            if(i <= availableProductIndex )
            {
                _productImages[i].sprite = productSprites[i];
                _productNames[i].text = productNames[i].ToString();
            }
            else
            {
                _productImages[i].sprite = lockedProductSprites[i];
                _productNames[i].text = null;
            }              

        }

        // Banner image assigning from the very last level image
        _productImages[productSprites.Length].sprite = productSprites[productSprites.Length - 1];

        base.Show(callback, parameters);
    }

    public override void Hide(Action callback = null)
    {
        base.Hide(callback);
    }

}
