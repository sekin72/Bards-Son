using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int HP;
    public int AC;
    public int attackBonus;
    public int damageBonus;
    public DamageDie DamageDie;

    public void Hurt(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Die();
        }
    }


    protected virtual void Die() { }
}
public enum DamageDie
{
    D4,
    D6,
    D8
}
