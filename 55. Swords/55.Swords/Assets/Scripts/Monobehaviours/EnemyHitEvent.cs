using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class EnemyHitEvent : MonoBehaviour, IPointerDownHandler
{
    public Events.EventGameObject OnClickAttackable;

    private GameObject enemyGamObject;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            RaycastHit hitInfo;
            Ray ray = eventData.pressEventCamera.ScreenPointToRay(eventData.pressPosition);
            if (Physics.Raycast(ray, out hitInfo))
            {
                enemyGamObject = hitInfo.collider.gameObject;
            }
            OnClickAttackable.Invoke(enemyGamObject);
        }
    } 
}



