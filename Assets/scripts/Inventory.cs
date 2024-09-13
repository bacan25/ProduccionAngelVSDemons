using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    private bool inventoryEnabled;
    public GameObject inventory;
    public GameObject instructions;
    public GameObject slotHolder;

    private int maxSlots;
    private int enabledSlots;
    private GameObject[] slots;
    public int slotInum;

    public GameObject itemIsUp;

    public bool inventoryOnStage;
    public bool instructionsOnStage;
    public bool instructionsEnabled;
    public int potionNum;

    public ItemManager itemManager;




    void Start()
    {
        

        maxSlots = slotHolder.transform.childCount;

        slots = new GameObject[maxSlots];

        for (int i = 0; i < maxSlots; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;

        }

        SlotInum();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryEnabled();
        }

        if (inventoryEnabled)
        {
            inventory.SetActive(true);
            inventoryOnStage = true;
        } else
        {
            inventory.SetActive(false);
            inventoryOnStage = false;
        }

        if (instructionsEnabled)
        {
            instructions.SetActive(true);
            instructionsOnStage = true;
        }
        else
        {
            instructions.SetActive(false);
            instructionsOnStage = false;
        }


    }

    


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Item")
        {
            itemIsUp = other.gameObject;

            Items item = itemIsUp.GetComponent<Items>();

            AddItem(itemIsUp, item.ID, item.type, item.descript, item.icon);

            if (item.GetComponent<Items>().ID == 1)
            {
                potionNum++;
                itemManager.potionTexto.text = potionNum.ToString();

            }


        }

        
    }

    public void AddItem(GameObject itemObject, int itemID, string itemType, string itemDescript, Sprite itemIcon)
    {
        //Debug.Log("AddItem" + maxSlots);

        for (int i = 0; i < maxSlots; i++)
        {
            //Debug.Log("----> " + slots[i].GetComponent<Slot>().empty);
            if (slots[i].GetComponent<Slot>().empty)
            {
                Debug.Log("Item");
                itemObject.GetComponent<Items>().isUp = true;
                

                slots[i].GetComponent<Slot>().item = itemObject;
                slots[i].GetComponent<Slot>().ID = itemID;
                slots[i].GetComponent<Slot>().type = itemType;
                slots[i].GetComponent<Slot>().descript = itemDescript;
                slots[i].GetComponent<Slot>().icon = itemIcon;

                itemObject.transform.parent = slots[i].transform;
                itemObject.SetActive(false);

                slots[i].GetComponent<Slot>().UpdateSlot();
                

                slots[i].GetComponent<Slot>().empty = false;

                

                return;

            }

            
        }
    }

    public void ClearOtherSlot()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            
            if (slots[i].GetComponent<Slot>().ID == itemManager.itemID && slots[i].GetComponent<Slot>().slotNum == itemManager.slotInum)
            {

                slots[i].GetComponent<Slot>().item = null;
                slots[i].GetComponent<Slot>().ID = 0;
                slots[i].GetComponent<Slot>().type = null;
                slots[i].GetComponent<Slot>().descript = null;
                slots[i].GetComponent<Slot>().icon = null;

                slots[i].GetComponent<Slot>().UpdateSlot();

                slots[i].GetComponent<Slot>().empty = true;

                return;

            } 



        }
    }

    public void AutoClearPotionSlot()
    {
        for (int i = 0; i < maxSlots; i++)
        {

            if (slots[i].GetComponent<Slot>().ID == 1)
            {
                slots[i].GetComponent<Slot>().item = null;
                slots[i].GetComponent<Slot>().ID = 0;
                slots[i].GetComponent<Slot>().type = null;
                slots[i].GetComponent<Slot>().descript = null;
                slots[i].GetComponent<Slot>().icon = null;

                slots[i].GetComponent<Slot>().UpdateSlot();

                slots[i].GetComponent<Slot>().empty = true;

                return;
            }



        }
    }


    public void SlotInum()
    {
        for (int i = 0; i < maxSlots; i++)
        {

            slots[i].GetComponent<Slot>().slotNum = slotInum;
            slotInum++;
        }
    }

    public void InventoryEnabled()
    {

        inventoryEnabled = !inventoryEnabled;

    }

    public void ScrollBarEnabled()
    {

        instructionsEnabled = !instructionsEnabled;


    }

    

  





}
