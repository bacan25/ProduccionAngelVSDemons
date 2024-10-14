using Photon.Pun;
using UnityEngine;

public class InventoryUpdate : MonoBehaviourPunCallbacks
{
    public ItemManager itemManager;
    public int allSlots;
    public Slot[] slots;

    private void Awake()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            // Si este no es el inventario del jugador local, desactivar el script
            enabled = false;
            return;
        }

        // Obtener referencia del ItemManager en el mismo jugador
        itemManager = GetComponentInParent<ItemManager>();
        if (itemManager == null)
        {
            Debug.LogError("ItemManager no encontrado. Asegúrate de que el InventoryUpdate está en el mismo jugador.");
        }
    }

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
