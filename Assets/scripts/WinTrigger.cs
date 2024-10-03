using Photon.Pun;
using UnityEngine;

public class WinTrigger : MonoBehaviourPun
{
    private bool gameEnded = false;
    public InGameManager inGameManager;

    private void Start()
    {
        if (inGameManager == null)
        {
            inGameManager = FindObjectOfType<InGameManager>();
            if (inGameManager == null)
            {
                Debug.LogError("InGameManager no encontrado en la escena. Asegúrate de que existe y está activo.");
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!gameEnded && col.CompareTag("Player"))
        {
            gameEnded = true;  // Evitar que se active varias veces
            PhotonView winnerView = col.GetComponent<PhotonView>();
            if (winnerView != null)
            {
                // Llamar RPC para manejar la victoria
                photonView.RPC("HandleWinRPC", RpcTarget.All, winnerView.ViewID);
            }
        }
    }

    [PunRPC]
    void HandleWinRPC(int winnerViewID)
    {
        PhotonView winnerView = PhotonView.Find(winnerViewID);
        if (winnerView != null)
        {
            PlayerCanvas winner = winnerView.GetComponent<PlayerCanvas>();

            // Verificar que inGameManager esté presente
            if (inGameManager == null)
            {
                inGameManager = FindObjectOfType<InGameManager>();
                if (inGameManager == null)
                {
                    Debug.LogError("InGameManager no encontrado en la escena durante HandleWinRPC.");
                    return;
                }
            }

            // Si se obtiene correctamente el PlayerCanvas del ganador, se maneja la victoria
            if (winner != null)
            {
                inGameManager.HandleWin(winner);
            }
            else
            {
                Debug.LogError("No se pudo obtener PlayerCanvas del ganador.");
            }
        }
        else
        {
            Debug.LogError("No se pudo encontrar PhotonView con ID: " + winnerViewID);
        }
    }
}
