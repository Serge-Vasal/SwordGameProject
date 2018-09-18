using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationAnimation : MonoBehaviour {

    NavMeshAgent agent;
    Animator anim;
	
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim=GetComponent<Animator>();
		}	
	
	void Update () {
        anim.SetFloat("Speed", agent.velocity.magnitude);
	}
}
