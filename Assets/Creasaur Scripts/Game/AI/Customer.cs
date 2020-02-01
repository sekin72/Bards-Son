using System;
using System.Threading;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;


public class Customer : PoolItem
{
    public ShopQueueController QueueManager;
    public CustomerBehaviourStates State { get; set; }

    public Patrol PatrolBehaviour;
    public WaitInQueue WaitInQueueBehaviour;
    public MoveToCashierBehaviour MoveToCashierBehaviour;
    public Shopping ShoppingBehaviour;
    public GoToExit GoToExitBehaviour;

    public override void OnInitialize()
    {
        base.OnInitialize();

        PatrolBehaviour = GetComponent<Patrol>();
        WaitInQueueBehaviour = GetComponent<WaitInQueue>();
        MoveToCashierBehaviour = GetComponent<MoveToCashierBehaviour>();
        ShoppingBehaviour = GetComponent<Shopping>();
        GoToExitBehaviour = GetComponent<GoToExit>();

        PatrolBehaviour.OnInitialize();
        WaitInQueueBehaviour.OnInitialize();
        MoveToCashierBehaviour.OnInitialize();
        ShoppingBehaviour.OnInitialize();
        GoToExitBehaviour.OnInitialize();
    }

    public override void Enable()
    {
        base.Enable();
    }

    public override void Disable()
    {
        GoToExitBehaviour.Disable();
        base.Disable();
    }

    public void SetTargetShopAndPatrol(Shop shop)
    {
        shop.AddCustomerToQueue(this);
        OnPatrolEnabled(shop.transform.GetChild(0).transform.position);
    }

    private void FixedUpdate()
    {
        if (!IsActive())
            return;
    }

    public void OnPatrolEnabled(Vector3 pos)
    {
        PatrolBehaviour.Enable();
        PatrolBehaviour.ShopPos = pos;
    }

    public void OnWaitInQueueEnabled(Vector3 firstWaitPos)
    {
        WaitInQueueBehaviour.Enable();
        PatrolBehaviour.Disable();
        WaitInQueueBehaviour.SendToWaitInQueue(firstWaitPos);
    }

    public void OnMoveOneLineForward(Vector3 pos)
    {
        WaitInQueueBehaviour.MoveOneLineForward(pos);
    }

    public void OnMoveToCashierEnabled(Cashier cashier, Vector3 pos)
    {
        MoveToCashierBehaviour.Enable();
        WaitInQueueBehaviour.Disable();
        PatrolBehaviour.Disable();
        MoveToCashierBehaviour.SendInFrontOfCashier(cashier, pos);
    }

    public void OnShoppingEnabled()
    {
        ShoppingBehaviour.Enable();
        MoveToCashierBehaviour.Disable();
    }

    public void OnGoToExitEnabled()
    {
        GoToExitBehaviour.Enable();
        WaitInQueueBehaviour.Disable();
        MoveToCashierBehaviour.Disable();
        ShoppingBehaviour.Disable();
        GoToExitBehaviour.SendToExit(QueueManager.GetExitPos());
    }

    public void OnGoToExitFinished()
    {
        GoToExitBehaviour.Disable();
        Disable();
    }
}