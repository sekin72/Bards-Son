using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text
{
    public Action AfterChoice;
    public string DisplayText;

    public Text(string text, Action onFinish)
    {
        AfterChoice = onFinish;
        DisplayText = text;
    }

    public void OnDisplayClosed()
    {
        AfterChoice();
    }
}
