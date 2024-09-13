using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public GameObject item;
    public int ID;
    public string type;
    public string descript;
    public Sprite icon;
    public bool empty = true;
    public Sprite iconDefault;
    public int slotNum;
    public int actualSlotNum;
    public ItemManager itemManager;

    public Transform slotIconPanel;
    public ItemDescript itemText;


    private void Start() 
    {
        

        slotIconPanel = transform.GetChild(0);
        iconDefault = slotIconPanel.GetComponent<Image>().sprite;
    }

    public void UpdateSlot()
    {
        slotIconPanel.GetComponent<Image>().sprite = icon== null? iconDefault: icon;
        //itemText.slotDescriptText.GetComponent<Text>().text = descript;  
        
    }

    public void UseItem()
    {
        item.GetComponent<Items>().ItemUsage();
        


    }

    public void CheckSlotNum()
    {
        actualSlotNum = slotNum;
        itemManager.slotInum = actualSlotNum;
        Debug.Log(itemManager.slotInum);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        UseItem();
        //CheckSlotNum();
        

    }
    
}
