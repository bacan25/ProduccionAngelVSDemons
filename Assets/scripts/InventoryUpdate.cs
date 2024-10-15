using Photon.Pun;
using UnityEngine;

public class InventoryUpdate : MonoBehaviourPunCallbacks
{
    public ItemManager itemManager;
    public int allSlots;
    public Slot[] slots;

    private void Start()
    {
        allSlots = this.transform.childCount;
        slots = new Slot[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slots[i] = this.transform.GetChild(i).gameObject.GetComponent<Slot>();
        }

        // Este script no se debe desactivar en el Awake, ya que no depende del jugador directamente.
        if (itemManager == null)
        {
            Debug.LogWarning("ItemManager aún no asignado al InventoryUpdate. Esto debe ser asignado por el ItemManager del jugador.");
        }
    }

    public void UpdateSlot()
    {
        if (itemManager == null)
        {
            Debug.LogError("ItemManager no asignado en InventoryUpdate. No se puede actualizar el inventario.");
            return;
        }

        for (int i = 0; i < allSlots; i++)
        {
            if (slots[i].slotID == itemManager.ID)
            {
                slots[i].SetNewImage();
                return;
            }
        }
    }

    // Método para asignar el ItemManager desde el jugador
    public void SetItemManager(ItemManager manager)
    {
        itemManager = manager;
    }
}
