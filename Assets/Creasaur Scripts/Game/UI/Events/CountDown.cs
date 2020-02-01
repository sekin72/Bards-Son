using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    private float _timeRemaining;
    [SerializeField] private Text _countDown;

    void OnEnable()
    {
        _timeRemaining = 60;
        _countDown.gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        _timeRemaining -= Time.deltaTime;
        _countDown.text = (int)(_timeRemaining / 60) + ":" + Second();
    }

    private string Second()
    {
        var seconds = (int)(_timeRemaining % 60);
        if (seconds < 10)
        {
            return "0" + seconds;
        }
        else
        {
            return seconds.ToString();
        }
    }

    void OnDisable()
    {
        _countDown.text = string.Empty;
    }
}
