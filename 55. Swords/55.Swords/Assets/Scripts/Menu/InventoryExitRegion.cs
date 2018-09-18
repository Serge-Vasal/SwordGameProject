using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryExitRegion : MonoBehaviour
{
    private Button invExitButton;

    private void Start()
    {
        invExitButton = gameObject.GetComponent<Button>();
        invExitButton.onClick.AddListener(StateToRunning);
    }

    private void StateToRunning()
    {        
        GameManager.Instance.StateToRunning(); 
    }     
    
}
