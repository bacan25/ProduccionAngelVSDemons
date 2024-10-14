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

        // Intentar obtener la referencia del ItemManager del jugador local
        GameObject localPlayer = GameObject.FindWithTag("Player"); // Asumiendo que el jugador tiene la etiqueta "Player"
        if (localPlayer != null)
        {
            itemManager = localPlayer.GetComponent<ItemManager>();
            if (itemManager == null)
            {
                Debug.LogError("ItemManager no encontrado en el jugador local. Asegúrate de que el script esté asignado correctamente.");
            }
        }
        else
        {
            Debug.LogError("Jugador local no encontrado en la escena. Asegúrate de que el objeto jugador esté presente.");
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
