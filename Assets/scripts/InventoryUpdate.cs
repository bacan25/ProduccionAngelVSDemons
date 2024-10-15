using Photon.Pun;
using UnityEngine;

public class InventoryUpdate : MonoBehaviourPunCallbacks
{
    public ItemManager itemManager;
    public int allSlots;
    public Slot[] slots;

    private void Awake()
    {
        // Este script solo se debe habilitar para el jugador local
        if (!PhotonNetwork.IsConnected || photonView.IsMine)
        {
            // Buscar el ItemManager del jugador local
            foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
            {
                var playerPhotonView = player.GetComponent<PhotonView>();
                if (playerPhotonView != null && playerPhotonView.IsMine)
                {
                    itemManager = player.GetComponent<ItemManager>();
                    break;
                }
            }

            if (itemManager == null)
            {
                Debug.LogError("ItemManager no encontrado para el jugador local. Asegúrate de que el jugador tiene el componente ItemManager.");
            }
        }
        else
        {
            // Si no es el jugador local, desactiva este script para evitar conflictos
            enabled = false;
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
