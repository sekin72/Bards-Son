using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using System;

public class PoolManager : MonoBehaviour
{
    public List<PoolItem> PooledObjects;
    public GameObject[] _prefabs;
    private int _customerAmountToPool = 30; // Get this from GameRules

    private GameObject _parentObject;
    private ShopQueueController _queueManager;

    private void Awake()
    {
        PooledObjects = new List<PoolItem>();
        MoreCustomerNeeded(_customerAmountToPool);
    }

    [Inject]
    public void Setup(ShopQueueController _queueManager)
    {
        this._queueManager = _queueManager;
    }

    public PoolItem GetActiveObjectFromPool()
    {
        return GetPooledObject() ?? MoreCustomerNeeded(10);
    }

    private PoolItem GetPooledObject()
    {
        var temp = PooledObjects.FirstOrDefault(x => !x.IsActive());
        return temp ? temp : null;
    }

    public PoolItem MoreCustomerNeeded(int _amount)
    {
        List<PoolItem> objList = GetNewInstances(PoolItemType.Customer, _customerAmountToPool);
        PooledObjects = PooledObjects.Union(objList).ToList();
        return objList[0];
    }

    public List<PoolItem> GetNewInstances(PoolItemType type, int _amount)
    {
        List<PoolItem> x = new List<PoolItem>();
        for (int i = 0; i < _amount; i++)
        {
            var obj = Create(type);
            obj.gameObject.transform.SetParent(gameObject.transform);
            obj.GetComponent<Customer>().QueueManager = _queueManager;    // Make this line suitable for both car and customer
            obj.OnInitialize();
            obj.Disable();
            x.Add(obj);
        }
        return x;
    }

    public PoolItem Create(PoolItemType type)
    {
        if (type == PoolItemType.Customer)
        {
            return Instantiate<GameObject>(_prefabs[0]).GetComponent<PoolItem>();

        }
        return Instantiate<GameObject>(_prefabs[0]).GetComponent<PoolItem>();// Will be Car Objects
    }
}
