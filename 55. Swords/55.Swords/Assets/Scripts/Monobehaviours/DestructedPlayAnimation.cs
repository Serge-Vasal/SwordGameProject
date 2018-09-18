using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedPlayAnimation : MonoBehaviour, IDestructible
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnDestruction(GameObject destroyer)
    {        
        anim.SetTrigger("Death");
    }

    public void ChangeGameStateToGameOver()
    {
        GameManager.Instance.StateToGameOver();
    }
}
