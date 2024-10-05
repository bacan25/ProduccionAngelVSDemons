using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

public class InGameManager : MonoBehaviourPunCallbacks
{
    public static InGameManager Instance;

    public Transform[] spawnPoints;
    public GameObject playerPrefab;
    public List<Transform> playerTransforms = new List<Transform>();

    private PlayerCanvas playerCanvas; // Referencia al canvas local

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // Si no está conectado a Photon, activar el modo offline
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.OfflineMode = true;
        }

        // Obtener la referencia al PlayerCanvas
        playerCanvas = PlayerCanvas.Instance;

        // Verificar si el PlayerCanvas está correctamente asignado
        if (playerCanvas == null)
        {
            Debug.LogError("PlayerCanvas no encontrado. Asegúrate de que el PlayerCanvas esté en la escena.");
        }

        // Spawnear al jugador
        SpawnPlayer();
    }


    void SpawnPlayer()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Los puntos de spawn no están asignados o la lista está vacía.");
            return;
        }

        // Obtener el índice de spawn para el jugador
        int spawnIndex = PhotonNetwork.OfflineMode ? 0 : PhotonNetwork.LocalPlayer.ActorNumber - 1;

        if (spawnIndex < spawnPoints.Length)
        {
            Vector3 spawnPosition = spawnPoints[spawnIndex].position;
            Quaternion spawnRotation = spawnPoints[spawnIndex].rotation;

            // Instanciar al jugador, ya sea en modo offline o en red
            GameObject player = PhotonNetwork.OfflineMode ?
                Instantiate(playerPrefab, spawnPosition, spawnRotation) :
                PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, spawnRotation);

            HealthSystem healthScript = player.GetComponent<HealthSystem>();
            if (healthScript != null)
            {
                healthScript.SetRespawnPosition(spawnPosition);
            }
            else
            {
                Debug.LogError("El componente HealthSystem no se encontró en el jugador instanciado.");
            }

            RegisterPlayer(player.transform);
        }
        else
        {
            Debug.LogError("No hay suficientes puntos de aparición para los jugadores.");
        }
    }

    public void RegisterPlayer(Transform playerTransform)
    {
        if (!playerTransforms.Contains(playerTransform))
        {
            playerTransforms.Add(playerTransform);
        }
    }

    public void UnregisterPlayer(Transform playerTransform)
    {
        if (playerTransforms.Contains(playerTransform))
        {
            playerTransforms.Remove(playerTransform);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Jugador {newPlayer.NickName} ha entrado a la sala.");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Jugador {otherPlayer.NickName} ha dejado la sala.");
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Has salido de la sala. Regresando al menú...");
        PhotonNetwork.LoadLevel("Menu");
    }

    // Manejo de la victoria del jugador
    public void HandleWin(PlayerCanvas winner)
    {
        if (playerCanvas == null)
        {
            Debug.LogError("PlayerCanvas no asignado en el jugador local.");
            return;
        }

        if (winner == playerCanvas)
        {
            // Mostrar el canvas de victoria del jugador local
            playerCanvas.WinCanvas();
            Debug.Log("El jugador local ha ganado.");
        }
        else
        {
            // Mostrar el canvas de derrota del jugador local
            playerCanvas.LoseCanvas();
            Debug.Log("El jugador local ha perdido.");
        }

        // Notificar en el log
        Debug.Log("El juego ha terminado. ¡Tenemos un ganador!");
    }

}
