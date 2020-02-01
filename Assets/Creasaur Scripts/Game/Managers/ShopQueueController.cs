using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timer;
using Zenject;
using System;
using UnityEngine.AI;
using System.Linq;

public interface IShopQueueController
{
    Vector3 GetExitPos();
    void AddNewCustomerToQueue(Customer customer);
    void CallCustomerFromPatrolToWaitingLine();
    Customer CallCustomerFromWaitingLineToCashier();
    void MoveWaitingCustomersOnePlaceForward();
}


public class ShopQueueController : IShopQueueController
{
    #region Variables
    private SimulationAreaManager _simulationAreaManager;
    public int CashierMaxCustomerCount;
    public Vector3 WaitingQueuePoint;
    public Vector3 WaitingQueueDirection;

    public bool RoomForMoreCustomerInTheWaitingLine;
    private bool CustomerPatrolsInTheShop;
    public bool CustomerWaitsInTheShop;

    private Action<Vector3> MoveOneLineForward;
    public Queue<Customer> PatrollingCustomersQueue;
    public Queue<Customer> WaitingCustomersQueue;
    #endregion


    [Inject]
    public ShopQueueController(SimulationAreaManager _simualtionAreaManager)
    {
        PatrollingCustomersQueue = new Queue<Customer>();
        WaitingCustomersQueue = new Queue<Customer>();
        CustomerPatrolsInTheShop = false;
        RoomForMoreCustomerInTheWaitingLine = true;
        this._simulationAreaManager = _simualtionAreaManager;
    }

    #region Getters/Setters

    public Vector3 GetExitPos()
    {
        return _simulationAreaManager.ExitPos;
    }

    #endregion

    public void FixedUpdate()
    {
        if ((RoomForMoreCustomerInTheWaitingLine && WaitingCustomersQueue.Count >= CashierMaxCustomerCount) ||
                (!RoomForMoreCustomerInTheWaitingLine && WaitingCustomersQueue.Count < CashierMaxCustomerCount))
        {
            RoomForMoreCustomerInTheWaitingLine = !RoomForMoreCustomerInTheWaitingLine;
        }

        if (CustomerPatrolsInTheShop && RoomForMoreCustomerInTheWaitingLine)
        {
            CallCustomerFromPatrolToWaitingLine();
        }
    }

    public void AddNewCustomerToQueue(Customer customer)
    {
        PatrollingCustomersQueue.Enqueue(customer);
        CustomerPatrolsInTheShop = true;
    }

    public void CallCustomerFromPatrolToWaitingLine()
    {
        if (PatrollingCustomersQueue.Count != 0)
        {
            var customer = PatrollingCustomersQueue?.Dequeue();
            if (PatrollingCustomersQueue.Count == 0)
            {
                CustomerPatrolsInTheShop = false;
            }
            customer.OnWaitInQueueEnabled(WaitingQueuePoint - 0.5f * WaitingCustomersQueue.Count * WaitingQueueDirection);
            WaitingCustomersQueue.Enqueue(customer);
            CustomerWaitsInTheShop = true;
        }
    }

    public Customer CallCustomerFromWaitingLineToCashier()
    {
        if (WaitingCustomersQueue.Count != 0)
        {
            var customer = WaitingCustomersQueue.Dequeue();
            if (WaitingCustomersQueue.Count == 0)
                CustomerWaitsInTheShop = false;
            return customer;
        }
        return null;
    }

    public void MoveWaitingCustomersOnePlaceForward()
    {
        for(int i=0;i< WaitingCustomersQueue.Count;i++)
        {

            MoveOneLineForward = WaitingCustomersQueue.ElementAt(i).OnMoveOneLineForward;
            MoveOneLineForward(WaitingQueuePoint - 0.5f * i * WaitingQueueDirection);
        }
    }
}
