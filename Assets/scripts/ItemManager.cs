using JetBrains.Annotations;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public int ID;
    public InventoryUpdate inventory;
    public Move_Player move_Player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            ID = other.GetComponent<Item>().itemID;
            inventory.UpdateSlot();
            Destroy(other.gameObject);

            if (ID == 1)
            {
                //move_Player.ActivateDoubleJump();
                PhotonView photonViewObj = other.GetComponent<PhotonView>();
                if (photonViewObj != null && photonViewObj.IsMine)
                {
                    PhotonNetwork.Destroy(other.gameObject);
                }
            }

            if (ID == 2)
            {
                //move_Player.ActivateClimb();
                PhotonView photonViewObj = other.GetComponent<PhotonView>();
                if (photonViewObj != null && photonViewObj.IsMine)
                {
                    PhotonNetwork.Destroy(other.gameObject);
                }
            }
        }
        
    }

    public void Comprar()
    {

    }

}
