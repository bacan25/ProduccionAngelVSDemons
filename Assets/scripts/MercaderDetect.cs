using Photon.Pun; // Añadir Photon para gestionar eventos de red
using UnityEngine;

public class MercaderDetect : MonoBehaviourPunCallbacks
{
    private PlayerCanvas playerCanvas;

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

    void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine) return; // Solo el jugador local debe mostrar el panel de la tienda

        if (other.CompareTag("Mercader"))
        {
            playerCanvas?.ShowMercaderPanel(photonView);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                int pocionPrecio = 10; // Precio de una poción
                playerCanvas?.ComprarPocion(pocionPrecio);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine) return; // Solo el jugador local debe ocultar el panel de la tienda

        if (other.CompareTag("Mercader"))
        {
            playerCanvas?.HideMercaderPanel(photonView);
        }
    }
}
