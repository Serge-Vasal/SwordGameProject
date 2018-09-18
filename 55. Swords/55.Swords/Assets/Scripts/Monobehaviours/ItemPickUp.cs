using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ItemPickUp : MonoBehaviour,IPointerDownHandler
{
    public EventVector3 OnClickedPickUpItem;
    public ItemPickUps_SO itemDefinition;
    public CharacterStats charStats;
    public bool inInventory;    
    CharacterInventory charInventory;

    Transform playerTransform;
    GameObject foundStats;   

    void Start()
    {
        if (charStats == null)
        {
            charInventory = CharacterInventory.instance;
            foundStats = GameObject.FindGameObjectWithTag("Player");
            charStats = foundStats.GetComponent<CharacterStats>();
            playerTransform = charStats.gameObject.transform;
        }        
    }

    void StoreItemInInventory()
    {        
        charInventory.StoreItem(this);
    }

    public void UseItem()
    {
        switch (itemDefinition.itemType)
        {
            case ItemTypeDefinitions.HEALTH:
                charStats.ApplyHealth(itemDefinition.itemAmount);                
                break;
            case ItemTypeDefinitions.MANA:
                charStats.ApplyMana(itemDefinition.itemAmount);
                break;
            case ItemTypeDefinitions.WEALTH:
                charStats.GiveWealth(itemDefinition.itemAmount);
                break;
            case ItemTypeDefinitions.WEAPON:                    
                charStats.ChangeWeapon(this);
                break;
            case ItemTypeDefinitions.ARMOR:
                charStats.ChangeArmor(this);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!inInventory&&other.tag == "Player")
        {
            if (itemDefinition.isStorable)
            {                
                StoreItemInInventory();
            }
            else
            {
                UseItem();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
        Vector3 agentToItemPickUpVector = transform.position - playerTransform.position;
        float agentToItemPickUpDistance = agentToItemPickUpVector.sqrMagnitude;       
        if (agentToItemPickUpDistance > 5f)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas);
            OnClickedPickUpItem.Invoke(hit.position);
        }
        else
        {
            if (!inInventory)
            {
                if (itemDefinition.isStorable)
                {
                    StoreItemInInventory();
                }
                else
                {
                    UseItem();
                }
            }
            
        }
    }
}
