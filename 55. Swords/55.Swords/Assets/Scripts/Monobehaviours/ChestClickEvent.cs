using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ChestClickEvent : MonoBehaviour, IPointerDownHandler
{
    public EventVector3 OnClickedChest;  
    public GameObject chestOpened;

    private SpawnItem spawnItemScript;
    private NavMeshAgent agent;
    private Vector3 clickedDestination;
    

    private void Start()
    {
        spawnItemScript = gameObject.GetComponentInChildren<SpawnItem>();
        agent = GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas);
        clickedDestination = hit.position;
        OnClickedChest.Invoke(hit.position);        
        StartCoroutine(AgentApproachChest());
    }

    IEnumerator AgentApproachChest()
    {
        do
        {
            Vector3 agentToChestVector = transform.position - agent.transform.position;
            float agentToChestDistance = agentToChestVector.sqrMagnitude;            
            if (agentToChestDistance<2f)
            {
                OpenChest();
                yield break;
            }         
            else if(clickedDestination != agent.destination)
            {
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
        while (clickedDestination==agent.destination);
    }

    private void OpenChest()
    {
        StopCoroutine(AgentApproachChest());
        chestOpened.SetActive(true);
        spawnItemScript.CreateSpawn();
        gameObject.SetActive(false);
    }
}
