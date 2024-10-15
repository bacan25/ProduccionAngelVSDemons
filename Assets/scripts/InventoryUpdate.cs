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

        allSlots = this.transform.childCount;
        slots = new Slot[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slots[i] = this.transform.GetChild(i).gameObject.GetComponent<Slot>();
        }
    }

    public void SetItemManager(ItemManager manager)
    {
        itemManager = manager;
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
                slots[i].SetNewImage(true); // Activar la imagen para indicar que el objeto está adquirido
                return;
            }
        }
    }

    public void UpdateSkills(Move_Player movePlayer, AngelClass angelClass)
    {
        if (movePlayer == null || angelClass == null)
        {
            Debug.LogError("Referencia de habilidades no encontrada en InventoryUpdate.");
            return;
        }

        // Actualizar las habilidades del jugador en los slots
        if (movePlayer.HasDoubleJump())
        {
            slots[0].SetNewImage(true); // Activar la imagen del doble salto
        }
        else
        {
            slots[0].SetNewImage(false); // Desactivar si la habilidad no está desbloqueada
        }

        if (movePlayer.HasClimb())
        {
            slots[1].SetNewImage(true); // Activar la imagen de escalar
        }
        else
        {
            slots[1].SetNewImage(false); // Desactivar si la habilidad no está desbloqueada
        }

        if (angelClass.HasPowerUp())
        {
            slots[2].SetNewImage(true); // Activar la imagen del power-up
        }
        else
        {
            slots[2].SetNewImage(false); // Desactivar si la habilidad no está desbloqueada
        }
    }
}
