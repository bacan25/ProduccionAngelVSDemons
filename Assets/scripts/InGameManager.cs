using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InGameManager : MonoBehaviourPunCallbacks
{
    public static InGameManager Instance;

    public Transform[] spawnPoints;
    public GameObject playerPrefab;
    public List<Transform> playerTransforms = new List<Transform>();

    private PlayerCanvas playerCanvas; // Referencia al canvas local

    // Listas de campamentos de enemigos de cada ruta
    [SerializeField] private GameObject ruta1Enemies;
    [SerializeField] private GameObject ruta2Enemies;

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
        if (PhotonNetwork.OfflineMode || (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom))
        {
            // Obtener la referencia al PlayerCanvas local
            playerCanvas = PlayerCanvas.Instance;

            // Spawnear al jugador y asignar ruta
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
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

            // Asignar la ruta de enemigos basada en el jugador que ha entrado
            if (spawnIndex == 0) // Primer jugador (Ruta 1)
            {
                ActivateRouteEnemies(ruta1Enemies);
            }
            else if (spawnIndex == 1) // Segundo jugador (Ruta 2)
            {
                ActivateRouteEnemies(ruta2Enemies);
            }
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
            Debug.Log($"Jugador registrado: {playerTransform.name}");
        }
    }

    public void UnregisterPlayer(Transform playerTransform)
    {
        if (playerTransforms.Contains(playerTransform))
        {
            playerTransforms.Remove(playerTransform);
            Debug.Log($"Jugador eliminado: {playerTransform.name}");
        }
    }

    // Método para activar enemigos de una ruta y desactivar los de la otra
    private void ActivateRouteEnemies(GameObject enemiesToActivate)
    {
        ruta1Enemies.SetActive(false);
        ruta2Enemies.SetActive(false);

        // Activar solo los enemigos de la ruta correspondiente
        enemiesToActivate.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Jugador {newPlayer.NickName} ha entrado a la sala.");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Jugador {otherPlayer.NickName} ha dejado la sala.");
        // Eliminar el transform del jugador que se fue
        GameObject player = GameObject.Find(otherPlayer.NickName);
        if (player != null)
        {
            UnregisterPlayer(player.transform);
        }
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
        if (playerCanvas != null && winner == playerCanvas)
        {
            // Mostrar el canvas de victoria del jugador local
            playerCanvas.WinCanvas();
        }
        else if (playerCanvas != null)
        {
            // Mostrar el canvas de derrota en el jugador local
            playerCanvas.LoseCanvas();
        }

        // Notificar en el log
        Debug.Log("El juego ha terminado. ¡Tenemos un ganador!");
    }
}
