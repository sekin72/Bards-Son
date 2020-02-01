using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    protected Vector3 _targetDir, _destinationPos;
    protected NavMeshPath _navMeshPath;
    protected int _cornerIndex = 0;
    bool flag = false;

    protected void CalculatePath(Vector3 targetPos)
    {
        _destinationPos = targetPos;
        NavMesh.CalculatePath(transform.position, targetPos, NavMesh.AllAreas, _navMeshPath);
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

            if (_navMeshPath.corners.Length == 0 && !flag)
            {
                var obj = Resources.Load("Prefabs/deneme");
                var gameobj = (GameObject)Instantiate(obj);
                gameobj.transform.position = _destinationPos;
                flag = true;
            }

            _targetDir = (_navMeshPath.corners[_cornerIndex]);
            transform.position = Vector3.MoveTowards(transform.position, _targetDir, step);

            /*Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 1, 1);
            transform.rotation = Quaternion.LookRotation(newDir);*/

            if (Vector3.Distance(transform.position, _targetDir) < 0.01f)
            {
                _cornerIndex++;
                if (_cornerIndex > _navMeshPath.corners.Length - 1)
                {
                    _cornerIndex = _navMeshPath.corners.Length - 1;
                }
            }
        }
        else if (_navMeshPath.corners.Length == 0 && GetType().ToString().Equals("Patrol"))
        {
            var patrolBeh = GetComponent<Patrol>();
            patrolBeh.SetNewRandomDestionationAroundShop();
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

