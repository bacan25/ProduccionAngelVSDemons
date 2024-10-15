using Photon.Pun; // Añadir Photon para gestionar eventos de red
using UnityEngine;

public class MercaderDetect : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject mercaderText;
    [SerializeField] private GameObject comprarPanel;
    private PlayerGoldManager playerGoldManager;

    private void Awake()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            enabled = false; // Asegurarse de que solo el jugador local ejecute este script
            return;
        }

        // Ya no necesitamos usar GameObject.Find. Asignar mercaderText y comprarPanel en el Inspector de Unity.
    }

    private void Start()
    {
        if (mercaderText != null)
            mercaderText.SetActive(false);

        if (comprarPanel != null)
            comprarPanel.SetActive(false);

        // Obtener la referencia al PlayerGoldManager del jugador
        playerGoldManager = GetComponent<PlayerGoldManager>();

        if (playerGoldManager == null)
        {
            Debug.LogError("PlayerGoldManager no encontrado en el jugador. Asegúrate de que el componente está asignado correctamente.");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine) return; // Solo el jugador local debe mostrar el panel de la tienda

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
        if (!photonView.IsMine) return; // Solo el jugador local debe ocultar el panel de la tienda

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
                PlayerCanvas.Instance.SumarPociones(); // Actualizar la UI de las pociones solo para el jugador local
                Debug.Log("Poción comprada con éxito!");
            }
            else
            {
                Debug.LogWarning("No tienes suficiente oro para comprar una poción.");
            }
        }
    }
}
