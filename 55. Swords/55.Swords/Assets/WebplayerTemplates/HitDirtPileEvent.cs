using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class HitDirtPileEvent : MonoBehaviour, IPointerDownHandler
{
    public Events.EventGameObject OnClickDirtPile;

    private GameObject dirtPile;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            RaycastHit hitInfo;
            Ray ray = eventData.pressEventCamera.ScreenPointToRay(eventData.pressPosition);
            if (Physics.Raycast(ray, out hitInfo))
            {
                dirtPile = hitInfo.collider.gameObject;
            }
            OnClickDirtPile.Invoke(dirtPile);
        }
    }


}
