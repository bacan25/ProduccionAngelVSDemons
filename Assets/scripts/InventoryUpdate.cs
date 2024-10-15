using Photon.Pun;
using UnityEngine;

public class InventoryUpdate : MonoBehaviourPunCallbacks
{
    public ItemManager itemManager;
    public int allSlots;
    public Slot[] slots;

    private void Awake()
    {
        // Desactivar el script si no pertenece al jugador local
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            enabled = false;
            return;
        }

        // Intentar obtener la referencia del ItemManager del jugador local usando una mejor manera de acceder al componente correcto
        itemManager = FindObjectOfType<ItemManager>(); // Busca el ItemManager en la escena (mejor en el contexto multijugador)
        if (itemManager == null)
        {
            Debug.LogError("ItemManager no encontrado. Asegúrate de que el script esté asignado correctamente al jugador local.");
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
}
