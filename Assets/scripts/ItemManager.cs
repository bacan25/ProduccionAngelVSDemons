using JetBrains.Annotations;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ItemManager : MonoBehaviour
{
    public int ID;
    public string nombre;
    public InventoryUpdate inventory;
    public Move_Player move_Player;

    public GameObject[] wings;
    public GameObject[] gloves;
    public GameObject[] hats;

    public GameObject uiPanel;
    public Text uiText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && PlayerCanvas.Instance.pociones >= 1)
        {
            PlayerCanvas.Instance.RestarPociones();
            //Sumar vida

        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            ID = other.GetComponent<Item>().itemID;
            nombre = other.GetComponent<Item>().itemName;
            inventory.UpdateSlot();
            //Destroy(other.gameObject);

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

            if (ID == 4)
            {
                StartCoroutine(ActivarYDesactivarPanel(3f));

                foreach (GameObject obj in wings)
                {
                    if (obj != null)  
                    {
                        Destroy(obj);
                    }
                }

                
            }
        }
        
    }

    IEnumerator ActivarYDesactivarPanel(float segundos)
    {
        uiText.text = "La reliquia " + nombre + " tiene un nuevo dueño.";
        
        uiPanel.SetActive(true);

        yield return new WaitForSeconds(segundos);

        uiPanel.SetActive(false);
    }

    public void Comprar()
    {
        if(PlayerCanvas.Instance.monedas >= 15)
        {
            PlayerCanvas.Instance.RestarMonedas();
            PlayerCanvas.Instance.SumarPociones();
        }
    }

}
