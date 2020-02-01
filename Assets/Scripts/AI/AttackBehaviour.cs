using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : Behaviour
{
    private float _attackTime, _currentTime;
    private bool _permissionToAttack;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.5f;

    public override void OnBehaviourChange()
    {
        enemy.State = BehaviourState.Attack;
        StartAttack();
    }

    private void StartAttack()
    {
        _permissionToAttack = true;
    }

    private void FixedUpdate()
    {
        if (_permissionToAttack)
        {
            _currentTime += Time.deltaTime;
            Collider[] enemies = Physics.OverlapSphere(_attackPoint.position, _attackRange, enemy._playerLayer);
            if (_currentTime >= _attackTime && enemies.Length == 1)
            {
                _currentTime = 0f;

                if (FRPSystem.RollD20(enemy.attackBonus, enemies[0].GetComponent<Player>().AC))
                {
                    enemies[0].GetComponent<Player>().Hurt(enemy.damageBonus);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
}
