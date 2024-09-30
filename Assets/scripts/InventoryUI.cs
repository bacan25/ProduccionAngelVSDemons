using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Transform slotsParent;
    public GameObject inventorySlotPrefab;

    private List<InventorySlotUI> slots = new List<InventorySlotUI>();

    private void Start()
    {
        // Inicializar los slots
        for (int i = 0; i < 20; i++)
        {
            GameObject slotObject = Instantiate(inventorySlotPrefab, slotsParent);
            InventorySlotUI slotUI = slotObject.GetComponent<InventorySlotUI>();
            slots.Add(slotUI);
        }
    }

    public void AddItem(Item item)
    {
        foreach (InventorySlotUI slot in slots)
        {
            if (!slot.HasItem())
            {
                slot.SetItem(item);
                return;
            }
        }
    }

    public void RemoveItemByID(int id)
    {
        foreach (InventorySlotUI slot in slots)
        {
            if (slot.GetItem() != null && slot.GetItem().ID == id)
            {
                slot.ClearSlot();
                return;
            }
        }
    }
}
