using Assets.Scripts.Game.Managers;
using Timer;
using UnityEngine;
using Zenject;

public class SpecialEvent : MonoBehaviour
{
    protected Coroutine timerToRemove;
    protected CreaCoroutine _timer;
    protected UserDataManager _userDataManager;
    protected SpawnManager _spawnManager; 
    
    private SpecialEventManager _specialEventManager;

    [Inject]
    public void Setup(CreaCoroutine timer, UserDataManager userDataManager, SpecialEventManager specialEventManager, SpawnManager spawnManager)
    {
        _timer = timer;
        _userDataManager = userDataManager;
        _specialEventManager = specialEventManager;
        _spawnManager = spawnManager;
    }

    public void SetCoroutine(Coroutine coroutine)
    {
        timerToRemove = coroutine;
    }

    public void DisableButton()
    {
        GetComponent<SpecialEventButton>().Enable = false;
    }

    public void EnableButton()
    {
        GetComponent<SpecialEventButton>().Enable = true;
    }

    public virtual void Activate()
    {
        _timer.StopTimer(timerToRemove);
        DisableButton();
    }

    public virtual void Deactivate()
    {
        EnableButton();
        _specialEventManager.OnRemoveSpecialEvent.SafeInvoke(this);
    }
}
