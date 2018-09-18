using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TerrainHitEvent : MonoBehaviour,IPointerDownHandler
{ 
    public EventVector3 OnClickEnvironment;

    private Vector3 worldClickPosition;

    public void OnPointerDown(PointerEventData eventData)
    { 
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {             
            RaycastHit hitInfo;
            Ray ray = eventData.pressEventCamera.ScreenPointToRay(eventData.pressPosition);
            if(Physics.Raycast(ray,out hitInfo))
            {
                worldClickPosition = hitInfo.point;
            }            
            OnClickEnvironment.Invoke(worldClickPosition);        
        }
    }
}

[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> { }


