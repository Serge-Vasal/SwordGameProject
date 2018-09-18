using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDefinition_SO : ScriptableObject
{ 
    public float CoolDown;

    public float Range;
    public float minDamage;
    public float maxDamage;
    public float criticalMultiplier;
    public float criticalChance;

    public Attack CreateAttack(CharacterStats wielderStats,CharacterStats defenderStats)
    {
        float coreDamage = wielderStats.GetDamage();
        coreDamage += Random.Range(minDamage, maxDamage);

        bool isCritical = Random.value < criticalChance;
        if (isCritical)
        {
            coreDamage *= criticalMultiplier;
        }

        if (defenderStats != null)
        {
            coreDamage -= defenderStats.GetResistance();
        }

        return new Attack((int)coreDamage, isCritical);
    }

	
}
