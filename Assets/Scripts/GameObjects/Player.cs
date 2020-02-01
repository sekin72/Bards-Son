using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Animator Animator;
    public bool Courage = false;
    public bool IdleStarted, RunningStarted, AttackStarted;

    protected override void Die()
    {

    }

    public void StartIdleAnim()
    {
        if (!IdleStarted)
        {
            if (Courage)
                Animator.Play("Idle", 0, 0);
            else
                Animator.Play("Terrified", 0, 0);
            IdleStarted = true;
            RunningStarted = false;
            AttackStarted = false;
        }
    }

    public void StartRunningAnim()
    {
        if (!RunningStarted)
        {
            IdleStarted = false;
            RunningStarted = true;
            AttackStarted = false;
            Animator.Play("Running", 0, 0);
        }
    }

    public void StartAttackAnim()
    {
        if (!AttackStarted)
        {
            IdleStarted = false;
            RunningStarted = true;
            AttackStarted = true;
            Animator.Play("Attacking", 0, 0);
        }
    }
}
