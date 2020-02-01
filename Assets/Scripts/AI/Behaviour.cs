using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Behaviour : MonoBehaviour
{
    public Enemy enemy;
    
    protected NavMeshPath _navMeshPath;
    protected int _cornerIndex = 0;
    bool flag = false;

    private void OnEnable()
    {
        _navMeshPath = new NavMeshPath();
        _cornerIndex = 0;
    }
    protected void CalculatePath(Vector3 targetPos)
    {
        enemy._destinationPos = targetPos;
        NavMesh.CalculatePath(transform.position, targetPos, NavMesh.AllAreas, _navMeshPath);

        //while (_navMeshPath.status == NavMeshPathStatus.PathInvalid)
        //    NavMesh.CalculatePath(transform.position, targetPos, NavMesh.AllAreas, _navMeshPath);
    }

    protected void Move(Color color)
    {
        if (_navMeshPath.status.Equals(NavMeshPathStatus.PathComplete))
        {
            float step = 2 * Time.deltaTime;

            for (int i = 0; i < _navMeshPath.corners.Length - 1; i++)
            {
                Debug.DrawLine(_navMeshPath.corners[i + 1], _navMeshPath.corners[i], color);
            }

            enemy._targetDir = (_navMeshPath.corners[_cornerIndex]);
            transform.position = Vector3.MoveTowards(transform.position, enemy._targetDir, step);

            /*Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 1, 1);
            transform.rotation = Quaternion.LookRotation(newDir);*/

            if (Vector3.Distance(transform.position, enemy._targetDir) < 0.01f)
            {
                _cornerIndex++;
                if (_cornerIndex > _navMeshPath.corners.Length - 1)
                {
                    _cornerIndex = _navMeshPath.corners.Length - 1;
                }
            }
        }
        else if (_navMeshPath.corners.Length == 0 && enemy.State == BehaviourState.Patrol)
        {
            enemy.PatrolBehaviour.SetNewRandomDestionationAroundShop();
        }
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

