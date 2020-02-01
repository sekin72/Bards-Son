using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : Behaviour
{
    public override void OnBehaviourChange()
    {
        StartPatrol();
    }

    private void StartPatrol()
    {
        CharacterController.Move(DecideNextPosition());
    }

    private Vector3 DecideNextPosition()
    {
        return Vector3.zero;
    }

}
