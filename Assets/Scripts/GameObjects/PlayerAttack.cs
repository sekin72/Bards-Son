using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player _player;
    private float _cooldown, _cooldownMax;

    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private LayerMask _enemyLayer;


    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider[] enemies = Physics.OverlapSphere(_attackPoint.position, _attackRange, _enemyLayer);
            for (int i = 0; i < enemies.Length; i++)
                Attack(enemies[i].GetComponent<Enemy>());
        }
    }
    public void Attack(Enemy enemy)
    {
        _cooldown -= Time.deltaTime;

        if (_cooldown <= 0)
            if (FRPSystem.RollD20(_player.attackBonus, enemy.AC))
            {
                enemy.Hurt(_player.damageBonus);
                _cooldown = _cooldownMax;
            }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
}
