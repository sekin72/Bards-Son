using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : Behaviour
{
    private Player _player;
    private float _attackTime = 3f, _currentTime = 0;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.5f;

    public override void OnBehaviourChange()
    {
        enemy.State = BehaviourState.Attack;
    }

    private void Awake()
    {
        _player = FindPlayerPosition();
    }
    private void FixedUpdate()
    {
        if (enemy.State == BehaviourState.Attack)
        {
            _currentTime += Time.deltaTime;
            Collider[] enemies = Physics.OverlapSphere(_attackPoint.position, _attackRange, enemy._playerLayer);
            if (_currentTime >= _attackTime && enemies.Length == 1)
            {
                enemy.Animator.Play("Attack", 0, 0);
                _currentTime = 0f;

                if (FRPSystem.RollD20(enemy.attackBonus, enemies[0].GetComponent<Player>().AC))
                {
                    enemies[0].GetComponent<Player>().Hurt(FRPSystem.RollDamage(enemy.DamageDie, enemy.damageBonus));
                }
            }
            if (Vector3.Distance(transform.position, _player.transform.position) > 3f)
            {
                enemy.MoveTowardsEnemyBehaviour.OnBehaviourChange();
            }
        }
    }

    private Player FindPlayerPosition()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
}
