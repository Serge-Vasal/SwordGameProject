using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : MonoBehaviour
{
    public Rigidbody RagdollCore;
    public float timeToLive;

    private Transform parent;

    public void ApplyForce(Vector3 force)
    {
        parent = gameObject.transform.parent;        
        RagdollCore.AddForce(force);
    }

    public void Disable()
    {
        StartCoroutine(CounterBeforeDisable());
    }

    private IEnumerator CounterBeforeDisable()
    {
        int i = 0;        
        while (i < 5)
        {
            i += 1;            
            yield return new WaitForSeconds(1);
        }
        gameObject.transform.parent = parent;
        yield break;
    }
}
