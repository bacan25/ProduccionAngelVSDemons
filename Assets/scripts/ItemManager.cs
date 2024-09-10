using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject activeItem = null;
    public int itemID;
    public string itemType;
    public int slotInum;

    public Health health;
    public Inventory clearSlot;

    private void Start()
    {
        //health = GetComponent<Health>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && itemType == "accesory")
        {
            Accesory();
            

        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(itemID == 1)
            {
                Potion();   
            }
            else
            {
                AutoPotion();
            }
        } 
    }

    private void Potion()
    {
        
        UsePotion();
        clearSlot.ClearOtherSlot();
        itemID = 0;



    }

    public void AutoPotion()
    {
        if (clearSlot.potionNum > 0)
        {
            UsePotion();
            clearSlot.AutoClearPotionSlot();
        }
       
    }

    private void UsePotion()
    {
        health.currentHealth += 10f;
        clearSlot.potionNum -= 1;
        

        if (health.currentHealth > health.maxHealth)
        {
            health.currentHealth = health.maxHealth;
        }


        if (activeItem != null)
        {
            if (itemID == 1)
            {
                activeItem.SetActive(false);
            }

            activeItem = null;

        }
    }
   

    private void Accesory()
    {
        if (itemID == 2)
        {
            Debug.Log("UseAcccesory");
            clearSlot.ClearOtherSlot();


            if (activeItem != null)
            {
                activeItem.SetActive(false);
                activeItem = null;
            }

        }
        itemID = 0;
    }


}
