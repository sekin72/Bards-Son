using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public PatrolBehaviour PatrolBehaviour;
    public MoveTowardsEnemyBehaviour MoveTowardsEnemyBehaviour;
    public AttackBehaviour AttackBehaviour;

    public Vector3 _targetDir, _destinationPos;
    public Vector3 StartPos;
    public BehaviourState State;
    public LayerMask _playerLayer;

    private void Awake()
    {
        StartPos = transform.position;
        _destinationPos = transform.position;

        PatrolBehaviour = GetComponent<PatrolBehaviour>();
        MoveTowardsEnemyBehaviour = GetComponent<MoveTowardsEnemyBehaviour>();
        AttackBehaviour = GetComponent<AttackBehaviour>();

        PatrolBehaviour.OnBehaviourChange();
    }
    protected override void Die()
    {

    }
}
