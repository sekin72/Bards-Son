using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRPSystem
{
    public static bool RollD20(int bonus, int DC)
    {
        return bonus + Random.Range(1, 21) >= DC;
    }
    public static int RollDamage(DamageDie damageDie, int damageBonus)
    {
        switch (damageDie)
        {
            case DamageDie.D4:
                return RollD4(damageBonus);
            case DamageDie.D6:
                return RollD6(damageBonus);
            case DamageDie.D8:
                return RollD8(damageBonus);
            default:
                return 0;
        }
    }

    private static int RollD4(int bonus)
    {
        return bonus + Random.Range(1, 5);
    }
    private static int RollD6(int bonus)
    {
        return bonus + Random.Range(1, 7);
    }
    private static int RollD8(int bonus)
    {
        return bonus + Random.Range(1, 9);
    }
}
