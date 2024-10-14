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
    public InventoryUpdate inventoryUpdate;
    public InventoryUpdate skillsUpdate;
    public GameObject inventory;
    public GameObject skills;
    public Move_Player move_Player;

    public GameObject[] wings;
    public GameObject[] gloves;
    public GameObject[] hats;

    public GameObject uiPanel;
    public Text uiText;

    public int pociones;
    

    private void Awake()
    {
        inventory = GameObject.Find("PanelInventario");
        inventoryUpdate = inventory.GetComponent<InventoryUpdate>();

        skills = GameObject.Find("Skills");
        skillsUpdate = inventory.GetComponent<InventoryUpdate>();

        uiPanel = GameObject.Find("PanelInventario");
        uiText = uiPanel.GetComponent<Text>();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && pociones >= 1)
        {
            pociones -= 1;
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
            inventoryUpdate.UpdateSlot();
            skillsUpdate.UpdateSlot();
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

            if (ID == 8)
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
        uiText.text = "La reliquia " + nombre + " tiene un nuevo due�o.";
        
        uiPanel.SetActive(true);

        yield return new WaitForSeconds(segundos);

        uiPanel.SetActive(false);
    }

    public void Comprar()
    {
        
    }

}
