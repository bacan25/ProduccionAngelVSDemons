using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject activeItem = null;
    public int itemID;

    public Health health;
    public Inventory clearSlot;

    private void Start()
    {
        //health = GetComponent<Health>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Potion();
        }
    }

    private void Potion()
    {
        if(itemID == 1 || clearSlot.potionNum > 0)
        {
            health.currentHealth += 10f;
            clearSlot.potionNum -= 1;
            clearSlot.ClearSlot();
            

            if (activeItem != null)
            {
                activeItem.SetActive(false);
                activeItem = null;  
            }

        }
    }


}
