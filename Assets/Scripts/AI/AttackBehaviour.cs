using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : Behaviour
{
    private float _attackTime, _currentTime;
    private bool _permissionToAttack;
    public override void OnBehaviourChange()
    {
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
            if (_currentTime >= _attackTime)
            {
                _currentTime = 0f;

                if (FRPSystem.RollD20(enemy.attackBonus, player.AC))
                {
                    player.Hurt(enemy.damageBonus);
                }
            }
        }
    }

}
