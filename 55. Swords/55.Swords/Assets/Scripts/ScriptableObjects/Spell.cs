using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Spell.asset",menuName ="Attack/Spell")]
public class Spell : AttackDefinition_SO
{
    public GameObject ProjectilePrefab;
    public GameObject projectileInstance;
    public Projectile projectileScript;
    public float ProjectileSpeed;   

    public void Cast(GameObject Caster, Vector3 HotSpot,Vector3 Target)
    {
        //Projectile projectile = Instantiate(ProjectileToFire, HotSpot, Quaternion.identity);
        projectileInstance.SetActive(true);
        projectileInstance.transform.position = HotSpot;
        projectileScript.Fire(Caster, Target, ProjectileSpeed, Range);        
    }    

    public void OnProjectileCollided(GameObject Caster,GameObject Target)
    {
        if (Caster == null || Target == null)
        {
            return;
        }

        var casterStats = Caster.GetComponent<CharacterStats>();
        var targetStats = Target.GetComponent<CharacterStats>();

        var attack = CreateAttack(casterStats, targetStats);

        var attackables = Target.GetComponentsInChildren(typeof(IAttackable));
        foreach(IAttackable a in attackables)
        {
            a.OnAttack(Caster, attack);
        }
    }


}
