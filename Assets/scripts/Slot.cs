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

    private void Start()
    {
        slotIconPanel = transform.GetChild(0);
        iconDefault = slotIconPanel.GetComponent<Image>().sprite;

        if (itemManager == null)
        {
            itemManager = FindObjectOfType<ItemManager>();
        }
    }

    public void UpdateSlot()
    {
        slotIconPanel.GetComponent<Image>().sprite = icon == null ? iconDefault : icon;
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.GetComponent<Items>().ItemUsage();
        }
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
        CheckSlotNum();
    }
}
