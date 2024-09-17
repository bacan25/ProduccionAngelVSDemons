using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class PlayerCanvasManager : MonoBehaviourPunCallbacks
{
    private Canvas playerCanvas;
    private Camera playerCamera;

    private void Start()
    {
        // Obtener la referencia del Canvas
        playerCanvas = GetComponent<Canvas>();

        // Solo el jugador local debe tener su canvas habilitado
        if (photonView.IsMine)
        {
            // Buscar la cámara dentro del prefab del jugador
            playerCamera = GetComponentInParent<Camera>();

            if (playerCamera == null)
            {
                Debug.LogError("No se encontró la cámara del jugador.");
                return;
            }

            // Configurar el Canvas para que se renderice solo en la cámara del jugador
            playerCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            playerCanvas.worldCamera = playerCamera;
            playerCanvas.planeDistance = 0.5f; // Ajusta la distancia del plano si es necesario
        }
        else
        {
            // Si este Canvas no pertenece al jugador local, desactivarlo
            playerCanvas.enabled = false;
        }
    }
}
