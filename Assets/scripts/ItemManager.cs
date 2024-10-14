using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        // Buscar el PanelInventario específicamente dentro del Canvas
        GameObject panelInventario = GameObject.Find("Canvas/HUB/PanelInventario");
        if (panelInventario != null)
        {
            inventoryUpdate = panelInventario.GetComponent<InventoryUpdate>();
            if (inventoryUpdate == null)
            {
                Debug.LogError("InventoryUpdate no encontrado en el objeto PanelInventario.");
            }
        }
        else
        {
            Debug.LogError("PanelInventario no encontrado en la jerarquía Canvas/HUB.");
        }

        // Buscar el PanelSkills específicamente dentro del Canvas si es necesario
        GameObject panelSkills = GameObject.Find("Canvas/HUB/PanelSkills");
        if (panelSkills != null)
        {
            skillsUpdate = panelSkills.GetComponent<InventoryUpdate>();
            if (skillsUpdate == null)
            {
                Debug.LogError("InventoryUpdate no encontrado en el objeto PanelSkills.");
            }
        }
        else
        {
            Debug.LogError("PanelSkills no encontrado en la jerarquía Canvas/HUB.");
        }

        // Buscar el PanelInventario para el panel de UI y el texto
        uiPanel = GameObject.Find("Canvas/HUB/PanelInventario");
        if (uiPanel != null)
        {
            uiText = uiPanel.GetComponentInChildren<Text>();
            if (uiText == null)
            {
                Debug.LogError("Text no encontrado en los hijos de PanelInventario.");
            }
        }
        else
        {
            Debug.LogError("PanelInventario no encontrado para uiPanel.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && pociones >= 1)
        {
            pociones -= 1;
            PlayerCanvas.Instance.RestarPociones();
            // Aquí puedes añadir lógica para incrementar la vida del jugador
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            ID = other.GetComponent<Item>().itemID;
            nombre = other.GetComponent<Item>().itemName;

            // Actualizar los slots de inventario y habilidades
            if (inventoryUpdate != null)
            {
                inventoryUpdate.UpdateSlot();
            }
            else
            {
                Debug.LogWarning("InventoryUpdate no está asignado. No se puede actualizar el inventario.");
            }

            if (skillsUpdate != null)
            {
                skillsUpdate.UpdateSlot();
            }
            else
            {
                Debug.LogWarning("SkillsUpdate no está asignado. No se pueden actualizar las habilidades.");
            }

            // Destruir el objeto de la escena si tiene un PhotonView y es propietario
            if (ID == 1 || ID == 2)
            {
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
        if (uiText != null)
        {
            uiText.text = "La reliquia " + nombre + " tiene un nuevo dueño.";
        }
        else
        {
            Debug.LogError("uiText no está asignado. No se puede mostrar el mensaje.");
        }

        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
            yield return new WaitForSeconds(segundos);
            uiPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("uiPanel no está asignado. No se puede activar el panel.");
        }
    }

    public void Comprar()
    {
        // Implementa lógica de compra aquí
    }
}
