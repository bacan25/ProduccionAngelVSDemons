using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class InGameManager : MonoBehaviourPunCallbacks
{
    public static InGameManager Instance; // Singleton para acceso global

    public Transform[] spawnPoints; // Asegúrate de que estén asignados en el inspector
    public GameObject playerPrefab; // Asegúrate de que esté asignado en el inspector
    public float disconnectCountdown = 5f;

    // Lista para almacenar los Transforms de los jugadores
    public List<Transform> playerTransforms = new List<Transform>();

    private void Awake()
    {
        // Implementación del singleton
        if (Instance == null)
        {
            Instance = this;
            // Opcional: DontDestroyOnLoad(gameObject); // Si deseas que el objeto persista entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        // Validación adicional de los spawn points
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Los puntos de spawn no están asignados o la lista está vacía.");
            return;
        }

        int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        if (spawnIndex < spawnPoints.Length)
        {
            Vector3 spawnPosition = spawnPoints[spawnIndex].position;
            Quaternion spawnRotation = spawnPoints[spawnIndex].rotation;

            // Instanciamos al jugador en red
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, spawnRotation);

            // Asignar la posición de respawn en el script Health del jugador
            Health healthScript = player.GetComponent<Health>();
            if (healthScript != null)
            {
                healthScript.SetRespawnPosition(spawnPosition); // Asegúrate de que SetRespawnPosition esté definido en Health.cs
            }
            else
            {
                Debug.LogError("El componente Health no se encontró en el jugador instanciado.");
            }
        }
        else
        {
            Debug.LogError("No hay suficientes puntos de aparición para los jugadores.");
        }
    }

    // Método para registrar un jugador
    public void RegisterPlayer(Transform playerTransform)
    {
        if (!playerTransforms.Contains(playerTransform))
        {
            playerTransforms.Add(playerTransform);
        }
    }

    // Método para eliminar un jugador de la lista
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
        SceneManager.LoadScene("Menu");
    }

    public void LoadLevel(string levelName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(levelName);
        }
    }

    // Método para manejar la victoria
    public void HandleWin(PlayerCanvas winner)
    {
        foreach (PlayerCanvas player in FindObjectsOfType<PlayerCanvas>())
        {
            if (player != winner)
            {
                player.LoseCanvas();
            }
            else
            {
                player.WinCanvas();
            }
        }
    }
}
