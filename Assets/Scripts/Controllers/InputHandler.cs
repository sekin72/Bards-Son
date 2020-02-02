using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    void Update()
    {
        if (GameManager.Instance.IsWaitingForInputForText)
        {
            if (Input.GetMouseButtonDown(1))
            {
                GameManager.Instance.currentText.AfterChoice();
            }
        }
    }
}
