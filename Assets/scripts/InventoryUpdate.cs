using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUpdate : MonoBehaviour
{


    public ItemManager itemManager;
    public int allSlots;
    public Slot[] slots;
    

    void Start()
    {
        allSlots = this.transform.childCount;
        slots = new Slot[allSlots];

        for (int i = 0; i < allSlots; i++) 
        {
            slots[i] = this.transform.GetChild(i).gameObject.GetComponent<Slot>();
        }


    }


    public void UpdateSlot()
    {
        for (int i = 0; i < allSlots; i++) 
        {
            if (slots[i].slotID == itemManager.ID)
            {
                
                slots[i].SetNewImage();
                return;
            }

            

        }
    }
}
