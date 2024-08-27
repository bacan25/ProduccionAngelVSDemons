using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class InGameManager : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoints;  // Array de puntos de aparición
    public GameObject playerPrefab;  // Prefab del jugador
    public float disconnectCountdown = 5f; // Cuenta regresiva en segundos
    public TMP_Text disconnectMessage; // Referencia al TMP que mostrará el mensaje

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }

        // Asegurarse de que el mensaje esté deshabilitado al inicio
        if (disconnectMessage != null)
        {
            disconnectMessage.gameObject.SetActive(false);
        }
    }

    void SpawnPlayer()
    {
        int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        if (spawnIndex < spawnPoints.Length)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        }
        else
        {
            Debug.LogError("No hay suficientes puntos de aparición para los jugadores.");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Jugador {newPlayer.NickName} ha entrado a la sala.");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Jugador {otherPlayer.NickName} ha dejado la sala.");
        StartCoroutine(HandlePlayerDisconnect());
    }

    private IEnumerator HandlePlayerDisconnect()
    {
        if (disconnectMessage != null)
        {
            disconnectMessage.gameObject.SetActive(true);
            disconnectMessage.text = $"Un jugador se ha desconectado. Terminando la partida en {disconnectCountdown} segundos...";
        }

        float countdown = disconnectCountdown;

        while (countdown > 0)
        {
            if (disconnectMessage != null)
            {
                disconnectMessage.text = $"Un jugador se ha desconectado. Terminando la partida en {countdown:F0} segundos...";
            }
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Menu");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Desconectado del servidor por la siguiente razón: {cause}");
        SceneManager.LoadScene("Menu");
    }


    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Has salido de la sala. Regresando al lobby...");
        SceneManager.LoadScene("Menu");
    }

    public void LoadLevel(string levelName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(levelName);
        }
    }
}
