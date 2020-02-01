using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItem : MonoBehaviour
{
    private PoolItemType _type;

    protected bool _isActive;

    public bool IsActive()
    {
        return _isActive;
    }

    public virtual void OnInitialize() { }
    public virtual void Enable()
    {
        _isActive = true;
        gameObject.SetActive(true);
    }
    public virtual void Disable()
    {
        _isActive = false;
        gameObject.SetActive(false);
    }
}

public enum PoolItemType
{
    Customer,
    Car,
    VIP_Customer
}