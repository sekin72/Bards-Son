using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    void Update()
    {
        if (GameManager.Instance.IsWaitingForInputForText)
        {
            if (Input.anyKeyDown)
            {
                GameManager.Instance.currentText.AfterChoice();
            }
        }
    }
}
