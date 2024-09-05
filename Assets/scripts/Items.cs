using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEditor.Progress;

public class Items : MonoBehaviour
{
    public int ID;
    public string type;
    public string descript;
    public Sprite icon;

    [HideInInspector]
    public bool isUp;

    [HideInInspector]
    public bool equiped;

    [HideInInspector]
    public GameObject accesoryManager;

    [HideInInspector]
    public GameObject accesory;

    public bool playerAccesory;
    public bool accesoryOnStage = false;
    int allAcessories;
    public ItemManager itemManager;
    private GameObject[] allItems;




    private void Start()
    {
        accesoryManager = GameObject.FindWithTag("AcessoryManager");
        allAcessories = accesoryManager.transform.childCount;
        allItems = new GameObject[allAcessories];


        if (!playerAccesory)
        {

            for (int i = 0; i < allAcessories; i++)
            {
                if (accesoryManager.transform.GetChild(i).gameObject.GetComponent<Items>().ID == ID)
                {
                    accesory = accesoryManager.transform.GetChild(i).gameObject;
                    //allItems[i] = accesory.transform.GetChild(i).gameObject;

                }
            }

        }


    }

    private void Update()
    {
        if (equiped)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                equiped = false;
            }
            if (equiped == false)
            {
                gameObject.SetActive(false);
            }
         
        }

        
    }

    public void ItemUsage()
    {
        //foreach (GameObject item in allItems)
        //{
        //    item.SetActive(false);
        //}

        if (type == "potion")
        {
            if(itemManager.activeItem != null)
            {
                itemManager.activeItem.SetActive(false);
                itemManager.activeItem = null;  
            }

            accesory.SetActive(true);
            accesory.GetComponent<Items>().equiped = true;
            itemManager.activeItem = accesory;
            itemManager.itemID = itemManager.activeItem.GetComponent<Items>().ID;


        }

        //if (type == "accesory")
        //{
        //    accesory.SetActive(true);
        //    accesory.GetComponent<Items>().equiped = true;
           

           
           
        //}
    }

  
}
