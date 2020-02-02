using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsEnemyBehaviour : Behaviour
{
    private Player _player;
    private float counter = 0f, timer = 1f;
    public override void OnBehaviourChange()
    {
        enemy.Animator.Play("Walk", 0, 0);
        enemy.State = BehaviourState.MoveTowardsEnemy;
        Vector3 newDestination = RandomNavSphere(_player.transform.position, 0);
        SendToLocation(newDestination);
    }

    private void Awake()
    {
        _player = FindPlayerPosition();
    }

    private void Update()
    {
        if (enemy.State == BehaviourState.MoveTowardsEnemy)
        {
            counter += Time.deltaTime;
            if (counter >= timer)
            {
                counter = 0f;
                Vector3 newDestination = RandomNavSphere(_player.transform.position, 0);
                SendToLocation(newDestination);
            }
            Move(Color.red);

            var x = Vector3.Distance(transform.position, _player.transform.position);
            if (x < 3f)
            {
                enemy.AttackBehaviour.OnBehaviourChange();
            }
        }
    }
    private Vector3 RandomNavSphere(Vector3 originPos, float dist)
    {
        var result = Vector3.zero;
        for (int i = 0; i < 30; i++)
        {
            var randomPoint = originPos + Random.insideUnitSphere * dist;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
            {
                result = hit.position;
                return result;
            }
        }
        return result;
    }

    private void SendToLocation(Vector3 targetPos)
    {
        _cornerIndex = 0;
        CalculatePath(targetPos);
    }

    private Player FindPlayerPosition()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

}
