﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Weapon.asset",menuName ="Attack/Weapon")]
public class Weapon : AttackDefinition_SO
{
    public GameObject weaponPreb;
    public GameObject weaponInstance;

    public void ExecuteAttack(GameObject attacker,GameObject defender)
    {
        if (defender == null)
            return;

        if (Vector3.Distance(attacker.transform.position, defender.transform.position) > Range)
            return;

        if (!attacker.transform.IsFacingTarget(defender.transform))
            return;

        var attackerStats = attacker.GetComponent<CharacterStats>();
        var defenderStats = defender.GetComponent<CharacterStats>();

        var attack = CreateAttack(attackerStats, defenderStats);

        var attackables = defender.GetComponentsInChildren(typeof(IAttackable));

        foreach (IAttackable attackable in attackables)
        {
            attackable.OnAttack(attacker, attack);
        }
    }
	
}
