using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class CallCustomerButton : MonoBehaviour
{
    private ShopQueueController _queueManager;

    /*[SerializeField]
    private ButtonBase _buttonBase;

    [Inject]
    public void Setup(QueueManager _queueManager)
    {
        this._queueManager = _queueManager;
    }*/

    private void Start()
    {
        //_buttonBase.OnClickEvent += SpawnCustomer;
    }
}
