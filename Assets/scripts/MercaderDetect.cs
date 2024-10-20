using Photon.Pun; // Añadir Photon para gestionar eventos de red
using UnityEngine;

public class MercaderDetect : MonoBehaviourPunCallbacks
{
    private PlayerCanvas playerCanvas;
    private bool isBuying = false;

    public PlayerGoldManager goldManager;


    private void Awake()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            enabled = false; // Asegurarse de que solo el jugador local ejecute este script
            return;
        }

        // Obtener referencia al PlayerCanvas del jugador local
        playerCanvas = PlayerCanvas.Instance;
        if (playerCanvas == null)
        {
            Debug.LogError("PlayerCanvas no encontrado. Asegúrate de que el Canvas del jugador esté en la escena.");
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1) && isBuying==true)
        {
            int pocionPrecio = 25; // Precio de una poción
            playerCanvas?.ComprarPocion(pocionPrecio);
            goldManager.SpendGold(25);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return; // Solo el jugador local debe mostrar el panel de la tienda
        isBuying = true;
        if (other.CompareTag("Mercader"))
        {
            playerCanvas?.ShowMercaderPanel();

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine) return; // Solo el jugador local debe ocultar el panel de la tienda
        isBuying=false;
        if (other.CompareTag("Mercader"))
        {
            playerCanvas?.HideMercaderPanel();
        }
    }
}
