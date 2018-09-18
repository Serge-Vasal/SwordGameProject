using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Aoe.asset")]
public class AOE_SO : AttackDefinition_SO
{
    public float Radius;
    
    public GameObject AoePrefab;

    public GameObject aoeInstance;
    public AoeAttackObject aoeScript;    

    public void CastAOE(GameObject Caster, Vector3 Position, int Layer)
    {         
        aoeInstance.transform.position = Position;
        aoeInstance.transform.rotation = Quaternion.identity;

        aoeInstance.SetActive(true); 

        var collidedObjects = Physics.OverlapSphere(Position, Radius);

        foreach (var collision in collidedObjects)
        {
            var collisionGo = collision.gameObject;

            if (Physics.GetIgnoreLayerCollision(Layer, collisionGo.layer))
            {
                continue;
            }

            var casterStats = Caster.GetComponent<CharacterStats>();
            var collisionStats = collisionGo.GetComponent<CharacterStats>();

            var attack = CreateAttack(casterStats, collisionStats);

            var attackables = collisionGo.GetComponentsInChildren(typeof(IAttackable));
            foreach (IAttackable a in attackables)
            {
                a.OnAttack(Caster, attack);
            }
            aoeScript.DeactivateAOE();
        }
    }    
}
