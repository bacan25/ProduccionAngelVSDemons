using Photon.Pun;
using UnityEngine;

public class PlayerGoldManager : MonoBehaviourPun
{
    public int currentGold;
    private PlayerCanvas playerCanvas;

    private void Awake()
    {
        // Asegurarse de que solo el jugador local maneje su propio oro
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            enabled = false;
            return;
        }

        // Buscar el PlayerCanvas en la escena para actualizar la UI
        playerCanvas = PlayerCanvas.Instance;

        if (playerCanvas == null)
        {
            Debug.LogError("PlayerCanvas no encontrado. Asegúrate de que PlayerCanvas esté en la escena.");
        }
    }

    // Método para añadir oro al jugador
    public void AddGold(int amount)
    {
        if (PhotonNetwork.OfflineMode)
        {
            currentGold += amount;
            UpdateGoldUI();
        }
        else if (photonView.IsMine)
        {
            photonView.RPC("RPC_AddGold", RpcTarget.AllBuffered, amount);
        }
    }

    [PunRPC]
    private void RPC_AddGold(int amount)
    {
        currentGold += amount;
        UpdateGoldUI();
    }

    // Método para restar oro cuando se compra una poción
    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateGoldUI();
            return true; // Compra exitosa
        }
        else
        {
            Debug.LogWarning("No hay suficiente oro para realizar la compra.");
            return false; // No hay suficiente oro
        }
    }

    // Método para actualizar la interfaz gráfica del oro
    private void UpdateGoldUI()
    {
        if (playerCanvas != null)
        {
            playerCanvas.UpdateGoldDisplay(currentGold);
        }
        else
        {
            Debug.LogError("PlayerCanvas no encontrado para actualizar la cantidad de oro.");
        }
    }
}
