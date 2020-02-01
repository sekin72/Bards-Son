using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour : MonoBehaviour
{
    public CharacterController CharacterController;
    public BehaviourState State;
    protected Enemy enemy;
    protected Player player;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        player = GetComponent<Player>();
    }
    public virtual void OnBehaviourChange() { }
}

public enum BehaviourState
{
    None,
    Patrol,
    MoveTowardsEnemy,
    Attack
}

