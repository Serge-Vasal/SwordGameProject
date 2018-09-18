using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAttackObject : MonoBehaviour
{
    private GameObject fireEffect;

    private void Awake()
    {
        fireEffect = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {  
        StartCoroutine(StompStartDelayed());
    }    

    public void DeactivateAOE()
    { 
        StartCoroutine(DeactivateAoe());
    }

    IEnumerator StompStartDelayed()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.07f);
        }
        fireEffect.SetActive(true);
        yield break;
    }

    IEnumerator DeactivateAoe()
    {
        for(int i = 0; i < 5; i++)
        {            
            yield return new WaitForSeconds(1);
        }
        fireEffect.SetActive(false);
        gameObject.SetActive(false);
        yield break;
    }

}
