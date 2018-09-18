using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollerScript : MonoBehaviour {

    NavMeshAgent agent;
    bool patrolling;
    int destPoint;
    bool arrived;
    Vector3 lastKnownPosition;
    Animator anim;

    public Transform[] patrolTargets;
    public Transform target;
    public Transform eye;
	
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        lastKnownPosition = transform.position;
	}	

    bool CanSeeTarget()
    {
        bool canSee = false;
        Ray ray = new Ray(eye.position, target.transform.position - eye.position);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            if (hit.transform != target)
            {
                canSee = false;
            }
            else
            {
                lastKnownPosition = target.transform.position;
                canSee = true;
            }
        }
        return canSee;
    }
	
	void Update () {
        if (agent.pathPending)
        {
            return;
        }

        if (patrolling)
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                if (!arrived)
                {
                    arrived = true;
                    StartCoroutine("GoToNextPoint");
                }        
            }
            else
            {
                arrived = false;
            }
        }
        if (CanSeeTarget())
        {
            agent.SetDestination(target.transform.position);
            patrolling = false;
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                anim.SetBool("Attack", true);
            }
            else
            {
                anim.SetBool("Attack", false);
            }
        }
        else
        {
            anim.SetBool("Attack", false);
            if (!patrolling)
            {
                agent.SetDestination(lastKnownPosition);
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    patrolling = true;
                    StartCoroutine("GoToNextPoint");
                }
            }            
        }	        
	}

    IEnumerator GoToNextPoint()
    {
        if (patrolTargets.Length == 0)
        {
            yield break;
        }
        patrolling = true;
        yield return new WaitForSeconds(2f);
        arrived = false;
        agent.destination = patrolTargets[destPoint].position;
        destPoint = (destPoint + 1) % patrolTargets.Length;

    }
}
