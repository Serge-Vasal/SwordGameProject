using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventory : MonoBehaviour
{
    #region Variable Declarations
    public static CharacterInventory instance;

    public CharacterStats charStats;

    //public Image[] hotBarDisplayHolders = new Image[4];    
    public GameObject inventoryDisplaySlotsHolder;
    public GameObject inventoryDisplayExitRegion;
    public Image[] inventoryDisplaySlots = new Image[30];

    [SerializeField] private Button invButton;

    int inventoryItemCap = 20;
    int idCount = 1;
    bool addedItem = true;    

    public Dictionary<int, InventoryEntry> itemsInInventory = new Dictionary<int, InventoryEntry>();
    public InventoryEntry itemEntry;
    #endregion

    #region Initializations
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        instance = this;

        itemEntry = new InventoryEntry(0, null, null);
        itemsInInventory.Clear();

        inventoryDisplaySlots = inventoryDisplaySlotsHolder.GetComponentsInChildren<Image>();

        charStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        }
        
        invButton.onClick.AddListener(ChangeGameStateToPausedInventory);
    }
    #endregion

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        invButton.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);
        inventoryDisplaySlotsHolder.SetActive(currentState == GameManager.GameState.PAUSEDINVENTORY);
        inventoryDisplayExitRegion.SetActive(currentState == GameManager.GameState.PAUSEDINVENTORY);
    }

    void ChangeGameStateToPausedInventory()
    {
        GameManager.Instance.StateToPausedInventory();
    }

    public void StoreItem(ItemPickUp ItemToStore)
    {
        addedItem = false;

        if ((charStats.characterDefinition.currentEncumbrance + ItemToStore.itemDefinition.itemWeight <= charStats.characterDefinition.maxEncumbrance))
        {
            itemEntry.invEntry = ItemToStore;
            itemEntry.stackSize = 1;
            itemEntry.hbSprite = ItemToStore.itemDefinition.itemIcon;
            ItemToStore.inInventory = true;
            
            ItemToStore.gameObject.SetActive(false);
            TryPickUp();
        }
    }

    void TryPickUp()
    {
        bool itsInInv = true;

        if (itemEntry.invEntry)
        {
            if (itemsInInventory.Count == 0)
            {
                addedItem = AddItemToInv(addedItem);
            }
            else
            {
                if (itemEntry.invEntry.itemDefinition.isStackable)
                {
                    foreach(KeyValuePair<int,InventoryEntry> ie in itemsInInventory)
                    {
                        if (itemEntry.invEntry.itemDefinition == ie.Value.invEntry.itemDefinition)
                        {
                            ie.Value.stackSize += 1;
                            //AddItemToHotBar(ie.Value);
                            itsInInv = true;
                            itemEntry.invEntry.gameObject.SetActive(false);
                            break;
                        }
                        else
                        {
                            itsInInv = false;
                        }
                    }
                }
                else
                {
                    itsInInv = false;

                    if (itemsInInventory.Count == inventoryItemCap)
                    {
                        itemEntry.invEntry.gameObject.SetActive(true);
                        Debug.Log("Inventory is full");
                        return;
                    }
                }

                if (!itsInInv)
                {
                    addedItem = AddItemToInv(addedItem);
                    itsInInv = true;
                }
            }
        }
    }

    bool AddItemToInv(bool finishedAdding)
    {
        //itemsInInventory.Add(idCount, new InventoryEntry(itemEntry.stackSize, Instantiate(itemEntry.invEntry), itemEntry.hbSprite));
        itemsInInventory.Add(idCount, new InventoryEntry(itemEntry.stackSize, itemEntry.invEntry, itemEntry.hbSprite));

        charStats.characterDefinition.currentEncumbrance += itemEntry.invEntry.itemDefinition.itemWeight;

        //Destroy(itemEntry.invEntry.gameObject);

        FillInventoryDisplay();
        //AddItemToHotBar(itemsInInventory[idCount]);

        idCount = IncreaseID(idCount);

        #region Reset itemEntry
        itemEntry.invEntry=null;
        itemEntry.stackSize = 0;        
        itemEntry.hbSprite = null;
        #endregion

        finishedAdding = true;

        return finishedAdding;
    }

    int IncreaseID(int currentID)
    {
        int newID = 1;

        for(int itemCount = 1; itemCount <= itemsInInventory.Count; itemCount++)
        {
            if (itemsInInventory.ContainsKey(newID))
            {
                newID += 1;
            }
            else return newID;
        }

        return newID;
    }

    //void AddItemToHotBar(InventoryEntry itemForHotBar)
    //{
    //    int hotBarCounter = 0;
    //    bool increaseCount = false;

    //    foreach(Image images in hotBarDisplayHolders)
    //    {
    //        hotBarCounter += 1;

    //        if (itemForHotBar.hotBarSlot == 0)
    //        {
    //            if (images.sprite == null)
    //            {
    //                itemForHotBar.hotBarSlot = hotBarCounter;
    //                images.sprite = itemForHotBar.hbSprite;
    //                increaseCount=true;
    //                break;
    //            }
    //        }
    //        else if (itemForHotBar.invEntry.itemDefinition.isStackable)
    //        {
    //            increaseCount = true;
    //        }
    //    }

    //    if (increaseCount)
    //    {
    //        hotBarDisplayHolders[itemForHotBar.hotBarSlot - 1].GetComponentInChildren<Text>().text =
    //            itemForHotBar.stackSize.ToString();
    //    }

    //    increaseCount = false;
    //}

    void DisplayInventory()
    {
        if (inventoryDisplaySlotsHolder.activeSelf == true)
        {
            inventoryDisplaySlotsHolder.SetActive(false);
        }
        else
        {
            inventoryDisplaySlotsHolder.SetActive(true);
        }
    }

    void FillInventoryDisplay()
    {
        int slotCounter = 9;

        foreach(KeyValuePair<int,InventoryEntry> ie in itemsInInventory)
        {
            slotCounter += 1;
            inventoryDisplaySlots[slotCounter].sprite = ie.Value.hbSprite;
            ie.Value.inventorySlot = slotCounter - 9;
        }

        while (slotCounter < 29)
        {
            slotCounter++;
            inventoryDisplaySlots[slotCounter].sprite = null;
        }
    }

    //public void TriggerItemUse(int itemToUseID)
    //{
    //    bool triggerItem = false;

    //    foreach(KeyValuePair<int,InventoryEntry> ie in itemsInInventory)
    //    {
    //        if (itemToUseID > 100)
    //        {
    //            itemToUseID -= 100;

    //            if (ie.Value.hotBarSlot== itemToUseID){
    //                triggerItem = true;
    //            }
    //        }
    //        else
    //        {
    //            if (ie.Value.inventorySlot == itemToUseID)
    //            {
    //                triggerItem = true;
    //            }
    //        }

    //        if (triggerItem)
    //        {
    //            if (ie.Value.stackSize == 1)
    //            {
    //                if (ie.Value.invEntry.itemDefinition.isStackable)
    //                {
    //                    //if (ie.Value.hotBarSlot != 0)
    //                    //{
    //                    //    hotBarDisplayHolders[ie.Value.hotBarSlot - 1].sprite = null;
    //                    //    hotBarDisplayHolders[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text = "0";
    //                    //}

    //                    ie.Value.invEntry.UseItem();
    //                    itemsInInventory.Remove(ie.Key);
    //                    break;                        
    //                }
    //                else
    //                {
    //                    ie.Value.invEntry.UseItem();
    //                    if (!ie.Value.invEntry.itemDefinition.isIndestructable)
    //                    {
    //                        itemsInInventory.Remove(ie.Key);
    //                        break;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                ie.Value.invEntry.UseItem();
    //                ie.Value.stackSize -= 1;
    //                //hotBarDisplayHolders[ie.Value.hotBarSlot - 1].GetComponentInChildren<Text>().text =
    //                //    ie.Value.stackSize.ToString();
    //            }
    //        }
    //    }
    //    FillInventoryDisplay();
    //}

    public void TriggerItemUse(int slotNumber)
    {       
        if (itemsInInventory.ContainsKey(slotNumber))
        {            
            itemsInInventory[slotNumber].invEntry.UseItem();
            FillInventoryDisplay();
        }
        
    }
}