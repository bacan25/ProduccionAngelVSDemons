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
            gameEnded = true;
            PhotonView winnerView = col.GetComponent<PhotonView>();
            if (winnerView != null)
            {
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

            if (inGameManager == null)
            {
                inGameManager = FindObjectOfType<InGameManager>();
                if (inGameManager == null)
                {
                    Debug.LogError("InGameManager no encontrado en la escena durante HandleWinRPC.");
                    return;
                }
            }

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
