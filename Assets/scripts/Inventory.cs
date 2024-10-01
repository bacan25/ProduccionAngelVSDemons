using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviourPun
{
    private int maxSlots = 20;
    private List<Item> items = new List<Item>();

    public GameObject inventoryUI; // Referencia al UI del inventario
    public bool inventoryOnStage; // Indica si el inventario está abierto
    public bool instructionsOnStage; // Indica si las instrucciones están abiertas

    public GameObject slotHolder;
    private GameObject[] slots;

    void Start()
    {
        if (!PhotonNetwork.OfflineMode && !photonView.IsMine)
        {
            enabled = false;
            return;
        }

        // Inicializar slots
        slots = new GameObject[maxSlots];
        for (int i = 0; i < maxSlots; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
            slots[i].GetComponent<Slot>().slotNum = i; // Asignar número de slot
        }
    }

    void Update()
    {
        if (!PhotonNetwork.OfflineMode && !photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            // Mostrar/ocultar inventario
            bool isActive = !inventoryUI.activeSelf;
            inventoryUI.SetActive(isActive);
            inventoryOnStage = isActive;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!PhotonNetwork.OfflineMode && !photonView.IsMine) return;

        if (other.gameObject.CompareTag("Item"))
        {
            Items itemComponent = other.gameObject.GetComponent<Items>();
            if (itemComponent != null)
            {
                Item item = new Item(itemComponent.ID, itemComponent.type, itemComponent.descript, itemComponent.icon);
                AddItem(item);
                
                // Arreglo de la línea de destrucción del objeto
                if (PhotonNetwork.OfflineMode)
                {
                    Destroy(other.gameObject); // Destruir el objeto localmente en modo offline
                }
                else
                {
                    PhotonNetwork.Destroy(other.gameObject); // Destruir el objeto en red
                }
            }
        }
    }

    public void AddItem(Item item)
    {
        if (items.Count < maxSlots)
        {
            items.Add(item);
            HUDManager.Instance.inventoryUI.GetComponent<InventoryUI>().AddItem(item);
        }
        else
        {
            Debug.Log("Inventario lleno");
        }
    }

    public void RemoveItemByID(int id)
    {
        Item itemToRemove = items.Find(item => item.ID == id);
        if (itemToRemove != null)
        {
            items.Remove(itemToRemove);
        }
        else
        {
            Debug.Log("Ítem no encontrado en el inventario.");
        }
    }

    public void ClearSlotByNumber(int slotNumber)
    {
        if (slotNumber >= 0 && slotNumber < slots.Length)
        {
            Slot slot = slots[slotNumber].GetComponent<Slot>();
            slot.item = null;
            slot.ID = 0;
            slot.type = null;
            slot.descript = null;
            slot.icon = null;

            slot.UpdateSlot();
            slot.empty = true;
        }
    }

    public int GetItemCount(int id)
    {
        int count = 0;
        foreach (Item item in items)
        {
            if (item.ID == id)
            {
                count++;
            }
        }
        return count;
    }
}
