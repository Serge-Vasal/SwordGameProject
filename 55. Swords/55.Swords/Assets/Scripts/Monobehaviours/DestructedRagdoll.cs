using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedRagdoll : MonoBehaviour, IDestructible
{
    public RagDoll RagdollObject;
    private RagDoll ragdollInstance; 
    public float Force;
    public float Lift;


    private void Awake()
    {       
        ragdollInstance = Instantiate(RagdollObject);        
        ragdollInstance.gameObject.transform.parent = this.gameObject.transform;
        ragdollInstance.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        ragdollInstance.gameObject.transform.localRotation = Quaternion.identity;
        ragdollInstance.gameObject.SetActive(false);
    }

    public void OnDestruction(GameObject destroyer)
    {  
        var vectorFromDestroyer = transform.position - destroyer.transform.position;
        vectorFromDestroyer.Normalize();
        vectorFromDestroyer.y += Lift;
        ragdollInstance.gameObject.SetActive(true);
        ragdollInstance.ApplyForce(vectorFromDestroyer * Force);
        ragdollInstance.gameObject.transform.parent = null;        
        ragdollInstance.Disable();  
      
        gameObject.SetActive(false);
    }
}
