using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUpdate : MonoBehaviour
{


    public ItemManager itemManager;
    public int allSlots;
    public GameObject[] slots;

    void Start()
    {
        allSlots = this.transform.childCount;
        slots = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++) 
        {
            slots[i] = this.transform.GetChild(i).gameObject;
        }


    }


    public void UpdateSlot()
    {
        for (int i = 0; i < allSlots; i++) 
        {
            if (slots[i].GetComponent<Slot>().slotID == itemManager.ID)
            {
                
                slots[i].GetComponent<Slot>().SetNewImage();
                return;
            }
            
        }
    }
}
