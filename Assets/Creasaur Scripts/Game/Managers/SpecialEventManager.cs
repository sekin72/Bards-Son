using System;
using System.Collections;
using System.Collections.Generic;
using Timer;
using UnityEngine;
using Zenject;


public class SpecialEventManager : MonoBehaviour
{
    private List<SpecialEvent> _activeSpecialEvents;
    private List<SpecialEvent> _inactiveSpecialEvents;
    public Action<SpecialEvent> OnRemoveSpecialEvent;
    //1 - VIP gemi çağırma: Direk para kazanımı
    //2 - Reklam kampanyası: Belirli bir süre insan akını
    //3 - Parti: Belirli bir süre x2 kazanç
    //4 - Çalışanlara Tip: Belirli bir süre anında satış / hazırlama
    //5 - Yolcu gemisi çağırma: Direk insan akını

    private float _time;
    private CreaCoroutine _timer;

    [Inject]
    public void Setup(SpecialEvent[] _specialEvents, CreaCoroutine timer)
    {
        _activeSpecialEvents = new List<SpecialEvent>();
        _inactiveSpecialEvents = new List<SpecialEvent>();
        _timer = timer;
        var _eventNumber = _specialEvents.Length;
        for(int i = 0; i < _eventNumber; i++)
        {
            _inactiveSpecialEvents.Add(_specialEvents[i]);
        }
        OnRemoveSpecialEvent = RemoveFirstEvent;
    }

    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= 1)
        {
            var randomEvent = LaunchEvent();
            //CallCustomer();
            _time = 0f;
            if(randomEvent != null)
            {
                randomEvent.SetCoroutine(_timer.Timer(TimeSpan.FromSeconds(30), OnRemoveSpecialEvent, randomEvent));
            }
        }
    }

    private SpecialEvent LaunchEvent()
    {
        if(_inactiveSpecialEvents.Count == 0)
        {
            return null;
        }

        var randomIndex = UnityEngine.Random.Range(0, _inactiveSpecialEvents.Count);
        SpecialEvent randomEvent = _inactiveSpecialEvents[randomIndex];
        _inactiveSpecialEvents.RemoveAt(randomIndex);
        randomEvent.transform.localPosition = new Vector3(434, 713, 0) + Vector3.down*140 * _activeSpecialEvents.Count;
        _activeSpecialEvents.Add(randomEvent);
        randomEvent.gameObject.SetActive(true);
        return randomEvent;
    }

    private void RemoveFirstEvent(SpecialEvent specialEvent)
    {
        _activeSpecialEvents.Remove(specialEvent);
        _inactiveSpecialEvents.Add(specialEvent);
        specialEvent.gameObject.SetActive(false);
        ArrangeEventOrder();
    }

    private void ArrangeEventOrder()
    {
        var length = _activeSpecialEvents.Count;
        for(int i=0; i<length; i++)
        {
            _activeSpecialEvents[i].gameObject.transform.localPosition = new Vector3(434, 713, 0) + Vector3.down * 140 * i;
        }
    }

}
