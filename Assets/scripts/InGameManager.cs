using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class InGameManager : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoints;  // Array de puntos de aparición
    public GameObject playerPrefab;  // Prefab del jugador
    public float disconnectCountdown = 5f; // Cuenta regresiva en segundos

    // Lista pública para almacenar los Transforms de los jugadores
    public List<Transform> playerTransforms = new List<Transform>();

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }

        // Otras inicializaciones, si es necesario...
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

    public void RegisterPlayerTransform(Transform playerTransform)
    {
        playerTransforms.Add(playerTransform);
        Debug.Log("Player registered. Total players: " + playerTransforms.Count);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Jugador {newPlayer.NickName} ha entrado a la sala.");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Jugador {otherPlayer.NickName} ha dejado la sala.");
        // Puedes manejar la lógica de quitar al jugador de la lista si es necesario
    }

    private IEnumerator HandlePlayerDisconnect()
    {
        Debug.Log("Un jugador se ha desconectado. Terminando la partida...");
        yield return new WaitForSeconds(disconnectCountdown);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("LobbyScene");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Desconectado del servidor por la siguiente razón: {cause}");
        SceneManager.LoadScene("LobbyScene");
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Has salido de la sala. Regresando al lobby...");
        SceneManager.LoadScene("LobbyScene");
    }

    public void LoadLevel(string levelName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(levelName);
        }
    }
}