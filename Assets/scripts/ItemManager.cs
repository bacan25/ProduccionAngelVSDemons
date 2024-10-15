using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviourPun
{
    public int ID;
    public string nombre;
    public InventoryUpdate inventoryUpdate;
    public GameObject inventory;
    public Move_Player move_Player;

    public GameObject[] wings;
    public GameObject[] gloves;
    public GameObject[] hats;

    public GameObject uiPanel;
    public Text uiText;

    public int pociones;

    private void Awake()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            // Asegurarse de que solo el jugador local maneje el inventario
            enabled = false;
            return;
        }

        // Solo el jugador local debe buscar estos componentes
        if (photonView.IsMine)
        {
            // Intentar encontrar el PanelInventario en la escena
            inventory = GameObject.Find("PanelInventario");
            if (inventory != null)
            {
                inventoryUpdate = inventory.GetComponent<InventoryUpdate>();
                if (inventoryUpdate != null)
                {
                    inventoryUpdate.SetItemManager(this); // Aquí se asigna el ItemManager al InventoryUpdate
                }
                else
                {
                    Debug.LogError("InventoryUpdate no encontrado en el objeto PanelInventario. Asegúrate de que PanelInventario tenga el componente InventoryUpdate.");
                }
            }
            else
            {
                Debug.LogError("PanelInventario no encontrado. Asegúrate de que el objeto PanelInventario esté en la escena.");
            }

            // Buscar el componente Text en los hijos de PanelInventario
            if (inventory != null)
            {
                uiText = inventory.GetComponentInChildren<Text>();
                if (uiText == null)
                {
                    Debug.LogError("Text no encontrado en los hijos de PanelInventario. Asegúrate de que haya un componente Text en uno de los hijos.");
                }
            }
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Q) && pociones >= 1)
        {
            pociones -= 1;
            PlayerCanvas.Instance.RestarPociones();
            // Lógica para sumar vida
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        if (other.CompareTag("Item"))
        {
            ID = other.GetComponent<Item>().itemID;
            nombre = other.GetComponent<Item>().itemName;

            if (inventoryUpdate != null)
            {
                inventoryUpdate.UpdateSlot();
            }
            else
            {
                Debug.LogError("InventoryUpdate no asignado en ItemManager. No se puede actualizar el inventario.");
            }

            // Destruir el item si tiene un PhotonView y el jugador es el propietario
            PhotonView photonViewObj = other.GetComponent<PhotonView>();
            if (photonViewObj != null && photonViewObj.IsMine)
            {
                PhotonNetwork.Destroy(other.gameObject);
            }

            // Realizar acciones según el ID del objeto
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
        if (uiText != null)
        {
            uiText.text = "La reliquia " + nombre + " tiene un nuevo dueño.";
            uiPanel.SetActive(true);

            yield return new WaitForSeconds(segundos);

            uiPanel.SetActive(false);
        }
    }
}
