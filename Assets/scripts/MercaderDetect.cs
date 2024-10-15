using Photon.Pun; // Añadir Photon para gestionar eventos de red
using UnityEngine;

public class MercaderDetect : MonoBehaviourPunCallbacks
{
    public GameObject mercaderText;
    public GameObject comprarPanel;
    private PlayerGoldManager playerGoldManager;

    private void Awake()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            enabled = false; // Asegurarse de que solo el jugador local ejecute este script
            return;
        }

        mercaderText = GameObject.Find("MercaderText");
        comprarPanel = GameObject.Find("ComprarPanel");
    }

    private void Start()
    {
        if (mercaderText != null)
            mercaderText.SetActive(false);
        
        if (comprarPanel != null)
            comprarPanel.SetActive(false);
        
        // Obtener la referencia al PlayerGoldManager del jugador
        playerGoldManager = GetComponent<PlayerGoldManager>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Mercader"))
        {
            if (mercaderText != null)
                mercaderText.SetActive(true);

            if (comprarPanel != null)
                comprarPanel.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ComprarPocion();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mercader"))
        {
            if (mercaderText != null)
                mercaderText.SetActive(false);

            if (comprarPanel != null)
                comprarPanel.SetActive(false);
        }
    }

    private void ComprarPocion()
    {
        int pocionPrecio = 10; // Precio de una poción

        if (playerGoldManager != null)
        {
            bool compraExitosa = playerGoldManager.SpendGold(pocionPrecio);
            if (compraExitosa)
            {
                PlayerCanvas.Instance.SumarPociones(); // Actualizar la UI de las pociones
                Debug.Log("Poción comprada con éxito!");
            }
            else
            {
                Debug.LogWarning("No tienes suficiente oro para comprar una poción.");
            }
        }
    }
}
